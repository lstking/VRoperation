using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class addNetWork : MonoBehaviour
{
    

    private void start()
    {
		Network nw = new Network();
		nw.ConnectToServer();
    }

    private void updata()
    {

    }
}
