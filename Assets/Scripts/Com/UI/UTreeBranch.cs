using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace Com.UI {
    public class UTreeBranch: UTreeBaseItem {

        public GameObject go;
        public Transform tran;
        private Transform content;
        private VerticalLayoutGroup layerOutGroup;
        private Toggle toggle;
        private Button button;
        private Text textTitle;
        private Text textOpen;
		private LayoutElement layout;

        private bool isOpen;
        private bool isInvokeInner = true;
        public Action<UTreeNodeData, bool> OnBranchSel;

        public UTreeBranch() {
            go = GameObject.Instantiate(Resources.Load("Prefabs/treeBranch", typeof(GameObject))) as GameObject;
            tran = go.transform;
            layerOutGroup = go.GetComponentsInChildren<VerticalLayoutGroup>()[1];
            content = layerOutGroup.transform;
            toggle = go.GetComponentInChildren<Toggle>();
            button = go.GetComponentInChildren<Button>();
            textTitle = toggle.GetComponentInChildren<Text>();
            textOpen = button.GetComponentInChildren<Text>();
            toggle.onValueChanged.AddListener(OnSelected);
            button.onClick.AddListener(OnClick);
			layout = go.GetComponentInChildren<LayoutElement> ();
            OnItemSel = OnItemSelected;
        }

        public override UTreeNodeData Data {
            set {
                data = value;
                data.connectItem = this;
                isOpen = true;
                textTitle.text = data.Title;
                textOpen.text = isOpen ? "-" : "+";
				layout.preferredWidth = 40 + textTitle.preferredWidth + 10;
            }
            get {
                return data;
            }
        }

        public void SetParent(Transform paren) {
            tran.parent = paren;
        }


        public void SetChild(Transform child) {
            child.parent = content;
        }

        private void OnClick() {
            isOpen = !isOpen;
            textOpen.text = isOpen ? "-" : "+";
            content.gameObject.SetActive(isOpen);
        }

        private void OnSelected(bool isSel) {
            if (isInvokeInner)
            {
                //这里是处理，子节点勾选之后，tree本来需要处理的逻辑
                Debug.Log("onselect");
                OnBranchSel(data, isSel);
                //OnItemSelected(data, isSel);
            }
            //OnBranchSel(data,isSel);
        }

        private void OnItemSelected(UTreeNodeData data, bool isSel) {
            
            //这里处理这个节点勾选之后要做的事情，专门指外部处理，例如是显示某个外部的物体
            //if (isSel) {
            //    Debug.Log(" Sbranch " + data.Title);
            //} else {
            //    Debug.Log(" Nbranch " + data.Title);
            //}
            Debug.Log(" branch :" + data.Title + isSel);
            isInvokeInner = false;
           // data.Check = isSel;
            toggle.isOn = isSel;
            isInvokeInner = true;
        }
    }
}
