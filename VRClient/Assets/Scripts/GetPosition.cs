using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GetPosition : MonoBehaviour
{
    //Network nw = new Network();
    
    
    //unity禁止new继承了MonoBehaviour的类
    //Network nw;

    //private void start()
    //{
    //    nw = this.gameObject.AddComponent<Network>();
    //}
    

    float i = 0;
	public GameObject newclient;

    private void Update()
    {

        //每秒执行一次操作
        if (i >= 1)
        {
            //nw.localmessage = "1|" + this.GetComponent<Transform>().position.x.ToString() + "|" + this.GetComponent<Transform>().position.y.ToString() + "|" + this.GetComponent<Transform>().position.z.ToString();
            //nw.SendMessage(nw.localmessage);
            tempdata.localmessage = "1|" + this.GetComponent<Transform>().position.x.ToString() + "|" + this.GetComponent<Transform>().position.y.ToString() + "|" + this.GetComponent<Transform>().position.z.ToString();
			tempdata.sendlocal = true;
			Debug.Log (tempdata.localmessage);
            i = 0;
        }
        i += Time.deltaTime;

        if(tempdata.istriggered == true)
        {
            //nw.localmessage = "2|";
            //nw.SendMessage(nw.localmessage);
            tempdata.triggermessage = "2|";
            tempdata.sendtrigger = true;
        }

		if(tempdata.islogin == true)
		{
			newclient = (GameObject)Instantiate(Resources.Load("file")) as GameObject;
			//newclient = GameObject.FindWithTag("otherclient");
			//newclient = Resources.Load("Human/Prefabs/manBody_Manual") as GameObject;
			newclient.transform.position = new Vector3(0f,0.882f,0f);
			newclient.transform.localScale = new Vector3(5f, 5f, 5f);
			//newclient.gameObject.SetActive(true);
			newclient.transform.name = tempdata.clientname;
			tempdata.islogin = false;
		}
    }
}

