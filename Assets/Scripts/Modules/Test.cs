using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Com.UI;

public class Test : MonoBehaviour {

    public UTree tree;

    private List<UTreeNodeData> data1;
    private List<UTreeNodeData> data2;
    public Button btn;

    // Use this for initialization
    void Start () {
        
        UTreeNodeData a1111a = new UTreeNodeData(10011111,"a1111a");
        UTreeNodeData a1111 = new UTreeNodeData(1001111, "a1111");
        UTreeNodeData a111 = new UTreeNodeData(100111, "a111");
        UTreeNodeData a11 = new UTreeNodeData(10011, "a11");
		a111.AddChild (a1111);
        a111.AddChild(a1111a);
        a11.AddChild(a111);
        UTreeNodeData a1 = new UTreeNodeData(1001, "a1");
        a1.AddChild(a11);
        UTreeNodeData a2 = new UTreeNodeData(1002, "a2");
        UTreeNodeData a3 = new UTreeNodeData(1003, "a3");
        UTreeNodeData b1 = new UTreeNodeData(2001, "b1");
        UTreeNodeData b2 = new UTreeNodeData(2002, "b2");
        UTreeNodeData b3 = new UTreeNodeData(2003, "b3");
        UTreeNodeData c1 = new UTreeNodeData(3001, "c1");
        UTreeNodeData a = new UTreeNodeData(1000, "a");
        a.AddChild(a1);
        a.AddChild(a2);
        a.AddChild(a3);
        UTreeNodeData b = new UTreeNodeData(2000, "b");
        b.AddChild(b1);
        b.AddChild(b2);
        b.AddChild(b3);
        UTreeNodeData c = new UTreeNodeData(3000, "c");
        c.AddChild(c1);
        UTreeNodeData d = new UTreeNodeData(4000, "d");
        UTreeNodeData e = new UTreeNodeData(5000, "e");
        data1 = new List<UTreeNodeData>();
        data1.Add(a);
        data1.Add(b);
        data1.Add(c);
        data1.Add(d);
        data1.Add(e);
        //data1.Add(a);

        UTreeNodeData i1 = new UTreeNodeData(4001, "i1");
        UTreeNodeData i = new UTreeNodeData(4000, "i");
        i.AddChild(i1);
        UTreeNodeData j1 = new UTreeNodeData(5001, "j1");
        UTreeNodeData j = new UTreeNodeData(5000, "j");
        data2 = new List<UTreeNodeData>();
        data2.Add(i);
        data2.Add(j);
        btn.onClick.AddListener(delegate(){ SetTreeData2(data1); });
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetTreeData1() {
        tree.SetDataProvider(data1);
    }

    public void SetTreeData2(List<UTreeNodeData> nodes) {
        foreach (var item in nodes)
        {
            Debug.Log(item.Title+"   " +item.Check);
        }

        //tree.SetDataProvider(data2);
        //if (node.Children != null)
        //    foreach (UTreeNodeData node1 in node.Children)
        //    {
        //        if (node1.Children != null)
        //            SetTreeData2(node1);
        //        else
        //            Debug.Log(node1.Title +"状态是：" +node1.Check);
        //    }
    }
}
