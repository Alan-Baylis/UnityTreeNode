using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace Com.UI {
    public class UTree : MonoBehaviour {

        private List<UTreeLeaf> _leafPool;
        private List<UTreeBranch> _branchPool;
        private List<UTreeLeaf> _leafList;
        private List<UTreeBranch> _branchList;
        private List<UTreeNodeData> nodeData;

        public RectTransform recycle;
        public RectTransform content;
        public VerticalLayoutGroup layerOutGroup;

        void Start() {
            _leafPool = new List<UTreeLeaf>();
            _branchPool = new List<UTreeBranch>();
            _leafList = new List<UTreeLeaf>();
            _branchList = new List<UTreeBranch>();
            if(recycle == null) {
                GameObject obj = new GameObject();
                obj.name = "Recycle";
                obj.transform.parent = this.transform;
            }
            recycle.gameObject.SetActive(false);
        }

        #region 数据设置
        public void SetDataProvider(List<UTreeNodeData> value) {
            if (value == null) return;
            nodeData = value;
  
            int leafCount = 0, branchCount = 0;
            foreach (UTreeNodeData data in nodeData) {
                branchCount += GetBranchCount(data);
                leafCount += GetLeafCount(data);
            }

            int diffBranch = Mathf.Abs(_branchList.Count - branchCount);
            int diffLeaf = Mathf.Abs(_leafList.Count - leafCount);
            bool isBranchEnough = _branchList.Count >= branchCount;
            bool isLeafEnough = _leafList.Count >= leafCount;
            ResetBranchList(diffBranch, isBranchEnough);
            ResetLeafList(diffLeaf, isLeafEnough);

            ResetTreeItem();
        }
        #endregion

        #region //事件处理
        //这里处理的是某个节点勾选之后，连带要勾选的逻辑
        public void OnSelectLeaf(UTreeNodeData data, bool isSel) {
            data.Check = isSel;
            if (isSel)       
                OnSelectParents(data.Parent, isSel);
        }

        //这里处理的是某个节点勾选之后，连带要勾选的逻辑
        public void OnSelectBranch(UTreeNodeData data, bool isSel) {
           
            if (isSel)
                OnSelectParents(data.Parent, isSel);
            OnSelectChildren(data, isSel);
        }

        private void OnSelectParents(UTreeNodeData data, bool isSel) {
            if (data != null) {
                if (data.connectItem != null && data.connectItem.OnItemSel != null) {
                    data.connectItem.OnItemSel.Invoke(data, isSel);
                    data.Check = isSel;
                }
                if (data.Parent != null) {
                    OnSelectParents(data.Parent, isSel);
                }
            }
        }

        private void OnSelectChildren(UTreeNodeData data, bool isSel) {
            if(data != null) {
                data.Check = isSel;
                if (data.Children != null) {
                    foreach(UTreeNodeData child in data.Children) {
                        OnSelectChildren(child, isSel);
                        if (child.connectItem != null && child.connectItem.OnItemSel != null) {
                            
                            child.connectItem.OnItemSel.Invoke(child, isSel);
                        }
                    }
                }

            }
        }
        #endregion

        #region //树构建
        private int GetBranchCount(UTreeNodeData data) {
            int branchCount = 0;
            if (data.IsBranch) {
                branchCount++;
                foreach (UTreeNodeData branch in data.Children) {
                    if (branch.IsBranch) {
                        branchCount += GetBranchCount(branch);
                    }
                }
            }
            return branchCount;
        }

        private int GetLeafCount(UTreeNodeData data) {
            int leafCount = 0;
            if (data.IsLeaf) {
                leafCount++;
            } else {
                foreach (UTreeNodeData leaf in data.Children) {
                    if (leaf.IsLeaf) {
                        leafCount++;
                    } else {
                        leafCount += GetLeafCount(leaf);
                    }
                }
            }
            return leafCount;
        }

        private void ResetBranchList(int diffBranch, bool isBranchEnough) {
            while (diffBranch > 0) {
                UTreeBranch branch;
                if (isBranchEnough) {
                    branch = _branchList[_branchList.Count - 1];
                    _branchList.Remove(branch);
                    _branchPool.Add(branch);
                    branch.SetParent(recycle);
                } else {
                    if (_branchPool.Count > 0) {
                        branch = _branchPool[0];
                        _branchPool.RemoveAt(0);
                    } else {
                        branch = Activator.CreateInstance<UTreeBranch>();
                        branch.OnBranchSel += OnSelectBranch;
                    }
                    _branchList.Add(branch);
                    //branch.SetParent(content);//引起重置，先不执行
                }
                diffBranch--;
            }
        }

        private void ResetLeafList(int diffLeaf, bool isLeafEnough) {
            while (diffLeaf > 0) {
                UTreeLeaf leaf;
                if (isLeafEnough) {
                    leaf = _leafList[_leafList.Count - 1];
                    _leafList.Remove(leaf);
                    _leafPool.Add(leaf);
                    leaf.SetParent(recycle);
                } else {
                    if (_leafPool.Count > 0) {
                        leaf = _leafPool[0];
                        _leafPool.RemoveAt(0);
                    } else {
                        leaf = Activator.CreateInstance<UTreeLeaf>();
                        leaf.OnLeafSel += OnSelectLeaf;
                    }
                    _leafList.Add(leaf);
                }
                diffLeaf--;
            }
        }

        private void ResetTreeItem() {
            branchUsingIndex = 0;
            leafUsingIndex = 0;
            foreach (UTreeNodeData data in nodeData) {
                if (data.IsBranch) {
                    SetBranchData(data, null);
                } else {
                    SetLeafData(data, null);
                }
            }
        }

        private void SetBranchData(UTreeNodeData data, UTreeBranch pBranch) {
            UTreeBranch branch = GetOneBranch();
            branch.Data = data;
            if (pBranch != null)
                pBranch.SetChild(branch.tran);
            else
                branch.SetParent(content);
            foreach (UTreeNodeData childData in data.Children) {
                if (childData.IsBranch)
                    SetBranchData(childData, branch);
                else {
                    SetLeafData(childData, branch);
                }
            }
        }

        private void SetLeafData(UTreeNodeData data, UTreeBranch branch) {
            UTreeLeaf leaf = GetOneLeaf();
            leaf.Data = data;
            if (branch != null)
                branch.SetChild(leaf.tran);
            else
                leaf.SetParent(content);
        }

        private int branchUsingIndex = 0;
        private UTreeBranch GetOneBranch() {
            if (_branchList.Count > branchUsingIndex) {

            } else {
                branchUsingIndex = 0;
            }
            return _branchList[branchUsingIndex++];
        }

        private int leafUsingIndex = 0;
        private UTreeLeaf GetOneLeaf() {
            if (_leafList.Count > leafUsingIndex) {

            } else {
                leafUsingIndex = 0;
            }
            return _leafList[leafUsingIndex++];
        }
        #endregion
    }
}
