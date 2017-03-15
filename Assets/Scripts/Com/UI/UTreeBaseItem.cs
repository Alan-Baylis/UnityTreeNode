using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

namespace Com.UI {

    public class UTreeBaseItem {

        protected UTreeNodeData data;
        public Action<UTreeNodeData, bool> OnItemSel;

        public virtual UTreeNodeData Data { get; set; }
    }
}
