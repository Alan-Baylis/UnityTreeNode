using System.Collections.Generic;

namespace Com.UI {
    public class UTreeNodeData {

        private int id;
        private string title;
        private bool isLeaf = true;
        private UTreeNodeData parentData;
        private List<UTreeNodeData> childDataList;
        //private bool

        public UTreeBaseItem connectItem;//方便处理勾选逻辑（这样做耦合了view  data，其实不大好）

        public UTreeNodeData(int id,string title) {
            this.id = id;
            this.title = title;
            this.Check = true;
        }

        public string Title {
            get {
                return title;
            }
        }

        public int ID {
            get {
                return id;
            }
        }

        public bool Check
        {
            get;
            set;
        }
        public bool IsLeaf {
            get {
                return isLeaf;
            }
        }

        public bool IsBranch {
            get {
                return isLeaf == false;
            }
        }

        public List<UTreeNodeData> Children {
            get {
                return childDataList;
            }
        }

        public UTreeNodeData Parent {
            get {
                return parentData;
            }
        }

        public void AddChild(UTreeNodeData child) {
            child.parentData = this;
            childDataList = childDataList ?? new List<UTreeNodeData>();
            childDataList.Add(child);
            isLeaf = false;
        }
    }
}
