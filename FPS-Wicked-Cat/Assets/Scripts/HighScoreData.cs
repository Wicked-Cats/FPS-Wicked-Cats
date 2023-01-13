//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.AI;

//public class HighScoreData : MonoBehaviour
//{
//    [Header("------ Score board Container ------")]
//    public GameObject entrycontainer;
//    public GameObject entrytemplate;
//    public GameObject entryrank;
//    public GameObject entryscore;
//    public GameObject entrytimesurv;
//    public GameObject entryenemieskilled;
//    public GameObject entryuserdeathcount;
//    public GameObject entryusername;

//    public void Awake()
//    {

//        //entrycontainer = GameObject.Find("EntryContainer");
//        //entrytemplate = entrycontainer.Find("entrytemp");

//        entrycontainer.SetActive(false);


//        float templateheight = 30f;

//        for (int i = 0; i < 10; i++)
//        {
//            GameObject entrytransform = Instantiate( entrycontainer );
//            RectTransform entryrect = entrytransform.GetComponent<RectTransform>();
//            entryrect.anchoredPosition = new Vector2(0, -templateheight * i);
//            entrycontainer.SetActive(true);


//            //int rank = i + 1;

//            //string rankstring;

//            //switch (rank)
//            //{
//            //    default:
//            //        rankstring = rank + "th"; break;

//            //    case 1: rankstring = "1st"; break;
//            //    case 2: rankstring = "2nd"; break;
//            //    case 3: rankstring = "3rd"; break;

//            //}
//            //entrytransform.find("_pos").getcomponent<Textmeshprougui>().text = rankstring;

//            //int score = random.range(0, 100000);
//            //entrytransform.find("_scoretext").getcomponent<textmeshprougui>().text = score.tostring();

//            //string time = "12:00";
//            //entrytransform.find("_timesurvived").getcomponent<textmeshprougui>().text = time;
//            //int killcou = random.range(0, 100);
//            //entrytransform.find("_enemykills").getcomponent<textmeshprougui>().text = killcou.tostring();
//            //int userdeath = random.range(0, 100);
//            //entrytransform.find("_userdeathcount").getcomponent<textmeshprougui>().text = userdeath.tostring();
//            //string name = "jjjj";
//            //entrytransform.find("_username").getcomponent<textmeshprougui>().text = name;




//        }
//    }
//}
