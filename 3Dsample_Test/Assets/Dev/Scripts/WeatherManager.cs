using UnityEngine;
using System;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using System.Xml;

public class WeatherManager : MonoBehaviour
{
    static HttpClient client = new HttpClient();

    // ���������Ϳ� ���� string �� �Դϴ�.
    // Public data string value  
    int num_Time;
    string url;
    string base_Date;
    string base_Time;
    string get_Time;
    private DateTime exception_base_time = DateTime.Now;

    private void Awake()
    {
        num_Time = int.Parse(DateTime.Now.ToString("HH"));
        base_Date = DateTime.Now.ToString("yyyyMMdd");
    }

    // url�� �޾ƿ��� ���� �Լ�
    public void Geturl()
    {
        url = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst";
        url += "?ServiceKey=" + "uFr8i78P03dhu07b8P%2B6%2BggLH8dpuW%2BbyG0rRfXBN6OCbb8v0U%2BBWi%2BcLuzQltox4l9XSNXX7IaHFC9vmD2C7A%3D%3D";
        url += "&numOfRows=1000";             // �������� ��� ��(Default : 12)  //�� �� �κ� 36�� ����  
        url += "&pageNo=1";                 // ������ ��ȣ(Default : 1)
        url += "&dataType=XML";             // ���� �ڷ�����(XML, JSON)
                                            //url += "&ftype=ODAM";
        url += "&base_date=" + base_Date;   // ��û ��¥(yyMMdd)
        url += "&base_time=" + base_Time;   // ��û �ð�(HHmm)
        url += "&nx=55";                    // ��û ���� x��ǥ
        url += "&ny=127";                   // ��û ���� y�·�

        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";

        string result = string.Empty;
        HttpWebResponse test_Response;
        using (test_Response = request.GetResponse() as HttpWebResponse)
        {
            StreamReader reader = new StreamReader(test_Response.GetResponseStream());
            result = reader.ReadToEnd();
        }

        Debug.Log(result);
    }    

    private void Start()
    {
       Get_Base_Time(num_Time);
       Get_Get_Time(num_Time);
       Geturl();
       
       StartCoroutine(LoadData());

       Debug.Log("Time:" + num_Time + "base_Date:" + base_Date + "base_Time:" + base_Time);
       Debug.Log("getTime:" + get_Time);
    }

    private static readonly Dictionary<int, string> baseTimeMap = new()
    {
        { 3, "0200" }, { 4, "0200" }, { 5, "0200" },
        { 6, "0500" }, { 7, "0500" }, { 8, "0500" },
        { 9, "0800" }, { 10, "0800" }, { 11, "0800" },
        { 12, "1100" }, { 13, "1100" }, { 14, "1100" },
        { 15, "1400" }, { 16, "1400" }, { 17, "1400" },
        { 18, "1700" }, { 19, "1700" }, { 20, "1700" },
        { 21, "2000" }, { 22, "2000" }, { 23, "2000" },
        { 24, "2300" }, { 1, "2300" }, { 2, "2300" }
    };

    private static readonly Dictionary<int, string> getTimeMap = new()
    {
        { 1, "0100" }, { 2, "0200" }, { 3, "0300" }, { 4, "0400" }, { 5, "0500" },
        { 6, "0600" }, { 7, "0700" }, { 8, "0800" }, { 9, "0900" }, { 10, "1000" },
        { 11, "1100" }, { 12, "1200" }, { 13, "1300" }, { 14, "1400" }, { 15, "1500" },
        { 16, "1600" }, { 17, "1700" }, { 18, "1800" }, { 19, "1900" }, { 20, "2000" },
        { 21, "2100" }, { 22, "2200" }, { 23, "2300" }, { 24, "2400" }
    };

    public void Get_Base_Time(int time)
    {
        if (baseTimeMap.TryGetValue(time, out string result))
        {
            base_Time = result;
            if (time == 24 || time == 1 || time == 2)
            {
                base_Date = exception_base_time.ToString("yyyyMMdd");
            }
        }
        else
        {
            base_Time = "2300"; // �⺻��
        }
    }

    public void Get_Get_Time(int time)
    {
        get_Time = getTimeMap.TryGetValue(time, out string result) ? result : "2300";
    }

    IEnumerator LoadData()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        
        yield return request.SendWebRequest();

        if(request.error == null)
        {
            // ���� ������ ����ϱ� ����
            Debug.Log(request.downloadHandler.text);
        }

        XmlDocument xml = new XmlDocument();
        xml.Load(url);
        XmlNodeList xmResponse = xml.GetElementsByTagName("response");  //response �������� ����
        XmlNodeList xmlList = xml.GetElementsByTagName("item");

        //XML ���� �� ������ �ҷ��� 
        foreach (XmlNode node in xmResponse)
        {
            if (node["header"]["resultMsg"].InnerText.Equals("NORMAL_SERVICE")) // ���� ������ ���
            {
                foreach (XmlNode node1 in xmlList)  // <item> �� �о� ���̱�
                {
                    //��get_Time �ִ°� ���⼭ ���� �ð� �̾Ƽ� ���ϴ� ���� ���� 
                    if (node1["fcstTime"].InnerText.Equals(get_Time))
                    {
                        if (node1["category"].InnerText.Equals("SKY"))  // �ϴû����� ���
                        {
                            switch (node1["fcstValue"].InnerText)
                            {
                                case "1":
                                    Debug.Log("����");
                                    break;
                                case "3":
                                    Debug.Log("��������");
                                    break;
                                case "4":
                                    Debug.Log("�帲");
                                    break;
                                default:
                                    Debug.Log("�ش��ϴ� �ڷᰡ ����");
                                    break;
                            }
                        }

                        if (node1["category"].InnerText.Equals("PTY"))  // ���������� ���
                        {
                            switch (node1["fcstValue"].InnerText)
                            {
                                case "0":
                                    Debug.Log("����");
                                    break;
                                case "1":
                                    Debug.Log("��");
                                    break;
                                case "2":
                                    //��/��/��������
                                    Debug.Log("��/��/��������");
                                    break;
                                case "3":
                                    Debug.Log("��");
                                    break;
                                case "4":
                                    Debug.Log("�ҳ���");
                                    break;
                                default:
                                    Debug.Log("�ش��ϴ� �ڷᰡ �����ϴ�.");
                                    break;
                            }
                        }

                        if (node1["category"].InnerText.Equals("TMP"))  // ���� ��� �ҷ��� 
                        {
                            //Debug.Log("TMP ��"+ node1["fcstDate"].InnerText);
                            Debug.Log("���� TMP:" + node1["fcstValue"].InnerText);
                        }
                    }

                }
            }
            else
            {
                string apiErrorMsg = String.Empty;

                // API ���� ���� �޼��� ����
                apiErrorMsg = node["header"]["resultMsg"].InnerText switch
                {
                    "APPLICATION_ERROR" => "���ø����̼� ����",
                    "DB_ERROR" => "�����ͺ��̽� ����",
                    "NODATA_ERROR" => "������ ����",
                    "HTTP_ERROR" => "HTTP ����",
                    "SERVICETIME_OUT" => "���� �������",
                    "INVALID_REQUEST_PARAMETER_ERROR" => "�߸��� ��û �Ķ����",
                    "NO_MANDATORY_REQUEST_PARAMETERS_ERROR" => "�ʼ���û �Ķ���Ͱ� ����",
                    "NO_OPENAPI_SERVICE_ERROR" => "�ش� ���� API���񽺰� ���ų� ����",
                    "SERVICE_ACCESS_DENIED_ERROR" => "���� ���� �ź�",
                    "TEMPORARILY_DISABLE_THE_SERVICEKEY_ERROR" => "�Ͻ������� ����� �� ���� ���� Ű",
                    "LIMITED_NUMBER_OF_SERVICE_REQUESTS_EXCEEDS_ERROR" => "���� ��û����Ƚ�� �ʰ�",
                    "SERVICE_KEY_IS_NOT_REGISTERED_ERROR" => "��ϵ��� ���� ���� Ű",
                    "DEADLINE_HAS_EXPIRED_ERROR" => "���� ����� ���� Ű",
                    "UNREGISTERED_IP_ERROR" => "��ϵ��� ���� IP",
                    "UNSIGNED_CALL_ERROR" => "������� ���� ȣ��",
                    "UNKNOWN_ERROR" => "��Ÿ����", _=> "�ش��ϴ� ������ �������� ����",
                };

                // API ���� ���� �޼��� ���
                Debug.Log(apiErrorMsg);
            }
        }
    }
}

