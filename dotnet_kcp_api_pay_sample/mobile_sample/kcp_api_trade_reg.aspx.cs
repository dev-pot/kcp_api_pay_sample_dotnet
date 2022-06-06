using System;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Payment
{
    public partial class Kcp_api_trade_reg : System.Web.UI.Page
    {
        protected string site_cd;       // 사이트코드
        protected string kcp_cert_info; // 인증서 정보
        protected string ordr_idxx;     // 주문번호
        protected string good_mny;      // 결제 금액
        protected string good_name;     // 상품명
        protected string pay_method;    // 결제수단
        protected string Ret_URL;       // 리턴 URL

        protected string actionResult;  // pay_method에 매칭되는 값 (인증창 호출 시 필요)
        protected string van_code;      // (포인트,상품권 인증창 호출 시 필요)

        protected string target_URL;

        protected string req_data;
        protected string res_data;

        protected string res_cd;      // 응답코드
        protected string res_msg;     // 응답메세지
        protected string approvalKey; // 거래등록키
        protected string traceNo;     // 추적번호
        protected string PayUrl;      // 거래등록 PAY URL

        protected void Page_Load(object sender, EventArgs e)
        {
            /* ============================================================================== */
            /* =   거래등록 API URL                                                         = */
            /* = -------------------------------------------------------------------------- = */
            target_URL = "https://stg-spl.kcp.co.kr/std/tradeReg/register"; // 개발서버
            //target_URL = "https://spl.kcp.co.kr/std/tradeReg/register"; // 운영서버
            /* ============================================================================== */
            /* =  요청정보                                                                  = */
            /* = -------------------------------------------------------------------------- = */
            site_cd = Request.Form["site_cd"]; // 사이트코드
            // 인증서정보(직렬화)
            kcp_cert_info = "-----BEGIN CERTIFICATE-----MIIDgTCCAmmgAwIBAgIHBy4lYNG7ojANBgkqhkiG9w0BAQsFADBzMQswCQYDVQQGEwJLUjEOMAwGA1UECAwFU2VvdWwxEDAOBgNVBAcMB0d1cm8tZ3UxFTATBgNVBAoMDE5ITktDUCBDb3JwLjETMBEGA1UECwwKSVQgQ2VudGVyLjEWMBQGA1UEAwwNc3BsLmtjcC5jby5rcjAeFw0yMTA2MjkwMDM0MzdaFw0yNjA2MjgwMDM0MzdaMHAxCzAJBgNVBAYTAktSMQ4wDAYDVQQIDAVTZW91bDEQMA4GA1UEBwwHR3Vyby1ndTERMA8GA1UECgwITG9jYWxXZWIxETAPBgNVBAsMCERFVlBHV0VCMRkwFwYDVQQDDBAyMDIxMDYyOTEwMDAwMDI0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAppkVQkU4SwNTYbIUaNDVhu2w1uvG4qip0U7h9n90cLfKymIRKDiebLhLIVFctuhTmgY7tkE7yQTNkD+jXHYufQ/qj06ukwf1BtqUVru9mqa7ysU298B6l9v0Fv8h3ztTYvfHEBmpB6AoZDBChMEua7Or/L3C2vYtU/6lWLjBT1xwXVLvNN/7XpQokuWq0rnjSRThcXrDpWMbqYYUt/CL7YHosfBazAXLoN5JvTd1O9C3FPxLxwcIAI9H8SbWIQKhap7JeA/IUP1Vk4K/o3Yiytl6Aqh3U1egHfEdWNqwpaiHPuM/jsDkVzuS9FV4RCdcBEsRPnAWHz10w8CX7e7zdwIDAQABox0wGzAOBgNVHQ8BAf8EBAMCB4AwCQYDVR0TBAIwADANBgkqhkiG9w0BAQsFAAOCAQEAg9lYy+dM/8Dnz4COc+XIjEwr4FeC9ExnWaaxH6GlWjJbB94O2L26arrjT2hGl9jUzwd+BdvTGdNCpEjOz3KEq8yJhcu5mFxMskLnHNo1lg5qtydIID6eSgew3vm6d7b3O6pYd+NHdHQsuMw5S5z1m+0TbBQkb6A9RKE1md5/Yw+NymDy+c4NaKsbxepw+HtSOnma/R7TErQ/8qVioIthEpwbqyjgIoGzgOdEFsF9mfkt/5k6rR0WX8xzcro5XSB3T+oecMS54j0+nHyoS96/llRLqFDBUfWn5Cay7pJNWXCnw4jIiBsTBa3q95RVRyMEcDgPwugMXPXGBwNoMOOpuQ==-----END CERTIFICATE-----";
            ordr_idxx     = Request.Form["ordr_idxx"];
            good_mny      = Request.Form["good_mny"];
            good_name     = Request.Form["good_name"];
            pay_method    = Request.Form["pay_method"];
            Ret_URL       = Request.Form["Ret_URL"];
            /* ============================================================================== */
            actionResult  = Request.Form["ActionResult"];
            van_code      = Request.Form["van_code"];
            
            req_data = "{\"site_cd\" : \"" + site_cd + "\"," +
                                  "\"kcp_cert_info\":\"" + kcp_cert_info + "\"," +
                                  "\"ordr_idxx\":\"" + ordr_idxx + "\"," +
                                  "\"good_mny\":\"" + good_mny + "\"," +
                                  "\"good_name\":\"" + good_name + "\"," +
                                  "\"pay_method\":\"" + pay_method + "\"," +
                                  "\"Ret_URL\":\"" + Ret_URL + "\"," +
                                  "\"escw_used\":\"N\"," +
                                  "\"user_agent\":\"\"}";

            Console.WriteLine(req_data);
            // SSL/ TLS 보안 채널을 만들수 없습니다. 오류 발생 시 추가 (프레임워크 버전을 올렸음에도 안될 경우 추가)
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
    
            // API REQ
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(target_URL);
            req.Method         = "POST";
            req.ContentType    = "application/json";

            byte[] byte_req = Encoding.UTF8.GetBytes(req_data);
            req.ContentLength = byte_req.Length;

            Stream st = req.GetRequestStream();
            st.Write(byte_req, 0, byte_req.Length);
            st.Close();

            // API RES
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader st_read = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            res_data = st_read.ReadToEnd();

            st_read.Close();
            res.Close();

            // RES JSON DATA Parsing
            JObject json_data = JObject.Parse(res_data);
            
            res_cd      = json_data["Code"].ToString();
            res_msg     = json_data["Message"].ToString();
            approvalKey = json_data["approvalKey"].ToString();
            traceNo     = json_data["traceNo"].ToString();
            PayUrl      = json_data["PayUrl"].ToString();

        }
    }   
}