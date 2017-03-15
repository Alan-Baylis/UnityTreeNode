using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

namespace Com.UI {
    public class UTreeLeaf: UTreeBaseItem {

        public GameObject go;
        public Transform tran;
        private Toggle toggle;
        private Text textTitle;
		private LayoutElement layout;

        private bool isInvokeInner = true;
        public Action<UTreeNodeData, bool> OnLeafSel;

        public UTreeLeaf() {
            go = GameObject.Instantiate(Resources.Load("Prefabs/treeLeaf", typeof(GameObject))) as GameObject;
            tran = go.transform;
            toggle = go.GetComponentInChildren<Toggle>();
            textTitle = toggle.GetComponentInChildren<Text>();
            toggle.onValueChanged.AddListener(OnSelected);
			layout = go.GetComponent<LayoutElement> ();
            OnItemSel = OnItemSelected;
        }

        public override UTreeNodeData Data {
            set {
                data = value;
                data.connectItem = this;
                textTitle.text = data.Title;
				layout.preferredWidth = 20 + textTitle.preferredWidth + 10;
            }
            get {
                return data;
            }
        }

        public void SetParent(Transform paren) {
            tran.parent = paren;
        }

        private void OnSelected(bool isSel) {
            //if (isInvokeInner)
            //{
            //    //这里是处理，子节点勾选之后，tree本来需要处理的逻辑
            //    OnLeafSel(data, isSel);
            //    //OnItemSelected(data, isSel);
            //}
            OnLeafSel(data, isSel);

        }

        private void OnItemSelected(UTreeNodeData data, bool isSel) {

            //这里处理这个节点勾选之后要做的事情，专门指外部处理，例如是显示某个外部的物体
            //if (isSel) {
            //    Debug.Log(" Sleaf " + data.Title);
            //} else {
            //    Debug.Log(" Nleaf " + data.Title);
            //}
            Debug.Log("leaf  " + data.Title + isSel);
            isInvokeInner = false;
            //data.Check = isSel;

            toggle.isOn = isSel;
            isInvokeInner = true;
        }
    }
}
