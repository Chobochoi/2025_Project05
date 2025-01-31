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

    // 공공데이터에 관한 string 값 입니다.
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

    // url을 받아오기 위한 함수
    public void Geturl()
    {
        url = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst";
        url += "?ServiceKey=" + "uFr8i78P03dhu07b8P%2B6%2BggLH8dpuW%2BbyG0rRfXBN6OCbb8v0U%2BBWi%2BcLuzQltox4l9XSNXX7IaHFC9vmD2C7A%3D%3D";
        url += "&numOfRows=1000";             // 한페이지 결과 수(Default : 12)  //★ 이 부분 36로 변경  
        url += "&pageNo=1";                 // 페이지 번호(Default : 1)
        url += "&dataType=XML";             // 받을 자료형식(XML, JSON)
                                            //url += "&ftype=ODAM";
        url += "&base_date=" + base_Date;   // 요청 날짜(yyMMdd)
        url += "&base_time=" + base_Time;   // 요청 시간(HHmm)
        url += "&nx=55";                    // 요청 지역 x좌표
        url += "&ny=127";                   // 요청 지역 y좌료

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
            base_Time = "2300"; // 기본값
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
            // 정보 값들을 출력하기 위함
            Debug.Log(request.downloadHandler.text);
        }

        XmlDocument xml = new XmlDocument();
        xml.Load(url);
        XmlNodeList xmResponse = xml.GetElementsByTagName("response");  //response 기준으로 생성
        XmlNodeList xmlList = xml.GetElementsByTagName("item");

        //XML 파일 뜯어서 정보를 불러옴 
        foreach (XmlNode node in xmResponse)
        {
            if (node["header"]["resultMsg"].InnerText.Equals("NORMAL_SERVICE")) // 정상 응답일 경우
            {
                foreach (XmlNode node1 in xmlList)  // <item> 값 읽어 들이기
                {
                    //★get_Time 넣는곳 여기서 현재 시간 뽑아서 원하는 정보 뽑음 
                    if (node1["fcstTime"].InnerText.Equals(get_Time))
                    {
                        if (node1["category"].InnerText.Equals("SKY"))  // 하늘상태일 경우
                        {
                            switch (node1["fcstValue"].InnerText)
                            {
                                case "1":
                                    Debug.Log("맑음");
                                    break;
                                case "3":
                                    Debug.Log("구름많음");
                                    break;
                                case "4":
                                    Debug.Log("흐림");
                                    break;
                                default:
                                    Debug.Log("해당하는 자료가 없음");
                                    break;
                            }
                        }

                        if (node1["category"].InnerText.Equals("PTY"))  // 강수형태일 경우
                        {
                            switch (node1["fcstValue"].InnerText)
                            {
                                case "0":
                                    Debug.Log("없음");
                                    break;
                                case "1":
                                    Debug.Log("비");
                                    break;
                                case "2":
                                    //비/눈/진눈개비
                                    Debug.Log("비/눈/진눈개비");
                                    break;
                                case "3":
                                    Debug.Log("눈");
                                    break;
                                case "4":
                                    Debug.Log("소나기");
                                    break;
                                default:
                                    Debug.Log("해당하는 자료가 없습니다.");
                                    break;
                            }
                        }

                        if (node1["category"].InnerText.Equals("TMP"))  // 현재 기온 불러옴 
                        {
                            //Debug.Log("TMP 들어감"+ node1["fcstDate"].InnerText);
                            Debug.Log("현재 TMP:" + node1["fcstValue"].InnerText);
                        }
                    }

                }
            }
            else
            {
                string apiErrorMsg = String.Empty;

                // API 응답 에러 메세지 조사
                apiErrorMsg = node["header"]["resultMsg"].InnerText switch
                {
                    "APPLICATION_ERROR" => "어플리케이션 에러",
                    "DB_ERROR" => "데이터베이스 에러",
                    "NODATA_ERROR" => "데이터 없음",
                    "HTTP_ERROR" => "HTTP 에러",
                    "SERVICETIME_OUT" => "서비스 연결실패",
                    "INVALID_REQUEST_PARAMETER_ERROR" => "잘못된 요청 파라메터",
                    "NO_MANDATORY_REQUEST_PARAMETERS_ERROR" => "필수요청 파라메터가 없음",
                    "NO_OPENAPI_SERVICE_ERROR" => "해당 오픈 API서비스가 없거나 폐기됨",
                    "SERVICE_ACCESS_DENIED_ERROR" => "서비스 접근 거부",
                    "TEMPORARILY_DISABLE_THE_SERVICEKEY_ERROR" => "일시적으로 사용할 수 없는 서비스 키",
                    "LIMITED_NUMBER_OF_SERVICE_REQUESTS_EXCEEDS_ERROR" => "서비스 요청제한횟수 초과",
                    "SERVICE_KEY_IS_NOT_REGISTERED_ERROR" => "등록되지 않은 서비스 키",
                    "DEADLINE_HAS_EXPIRED_ERROR" => "기한 만료된 서비스 키",
                    "UNREGISTERED_IP_ERROR" => "등록되지 않은 IP",
                    "UNSIGNED_CALL_ERROR" => "서명되지 않은 호출",
                    "UNKNOWN_ERROR" => "기타에러", _=> "해당하는 에러가 존재하지 않음",
                };

                // API 응답 에러 메세지 출력
                Debug.Log(apiErrorMsg);
            }
        }
    }
}

