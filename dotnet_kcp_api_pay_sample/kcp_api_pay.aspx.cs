using System;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Runtime.ConstrainedExecution;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Engines;

namespace Payment
{
    public partial class Kcp_api_pay : System.Web.UI.Page
    {
        // 인증서 정보(직렬화)
        public string KCP_CERT_INFO = "-----BEGIN CERTIFICATE-----MIIDgTCCAmmgAwIBAgIHBy4lYNG7ojANBgkqhkiG9w0BAQsFADBzMQswCQYDVQQGEwJLUjEOMAwGA1UECAwFU2VvdWwxEDAOBgNVBAcMB0d1cm8tZ3UxFTATBgNVBAoMDE5ITktDUCBDb3JwLjETMBEGA1UECwwKSVQgQ2VudGVyLjEWMBQGA1UEAwwNc3BsLmtjcC5jby5rcjAeFw0yMTA2MjkwMDM0MzdaFw0yNjA2MjgwMDM0MzdaMHAxCzAJBgNVBAYTAktSMQ4wDAYDVQQIDAVTZW91bDEQMA4GA1UEBwwHR3Vyby1ndTERMA8GA1UECgwITG9jYWxXZWIxETAPBgNVBAsMCERFVlBHV0VCMRkwFwYDVQQDDBAyMDIxMDYyOTEwMDAwMDI0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAppkVQkU4SwNTYbIUaNDVhu2w1uvG4qip0U7h9n90cLfKymIRKDiebLhLIVFctuhTmgY7tkE7yQTNkD+jXHYufQ/qj06ukwf1BtqUVru9mqa7ysU298B6l9v0Fv8h3ztTYvfHEBmpB6AoZDBChMEua7Or/L3C2vYtU/6lWLjBT1xwXVLvNN/7XpQokuWq0rnjSRThcXrDpWMbqYYUt/CL7YHosfBazAXLoN5JvTd1O9C3FPxLxwcIAI9H8SbWIQKhap7JeA/IUP1Vk4K/o3Yiytl6Aqh3U1egHfEdWNqwpaiHPuM/jsDkVzuS9FV4RCdcBEsRPnAWHz10w8CX7e7zdwIDAQABox0wGzAOBgNVHQ8BAf8EBAMCB4AwCQYDVR0TBAIwADANBgkqhkiG9w0BAQsFAAOCAQEAg9lYy+dM/8Dnz4COc+XIjEwr4FeC9ExnWaaxH6GlWjJbB94O2L26arrjT2hGl9jUzwd+BdvTGdNCpEjOz3KEq8yJhcu5mFxMskLnHNo1lg5qtydIID6eSgew3vm6d7b3O6pYd+NHdHQsuMw5S5z1m+0TbBQkb6A9RKE1md5/Yw+NymDy+c4NaKsbxepw+HtSOnma/R7TErQ/8qVioIthEpwbqyjgIoGzgOdEFsF9mfkt/5k6rR0WX8xzcro5XSB3T+oecMS54j0+nHyoS96/llRLqFDBUfWn5Cay7pJNWXCnw4jIiBsTBa3q95RVRyMEcDgPwugMXPXGBwNoMOOpuQ==-----END CERTIFICATE-----";
        
        protected string tran_cd;
        protected string site_cd;
        protected string enc_data;
        protected string enc_info;
        protected string ordr_mony;

        protected string target_URL;
        protected string use_pay_method;
        protected string ordr_idxx;

        protected string req_data;
        protected string res_data;
        protected string bSucc;
        // 공통
        protected string res_cd;
        protected string res_msg;
        protected string res_en_msg;
        protected string tno;
        protected string amount;
        protected string app_time; // 공통(카드:승인시간,계좌이체:계좌이체시간,가상계좌:가상계좌 채번시간)
                                   // 카드
        protected string card_cd; // 카드코드
        protected string card_name; // 카드사
        protected string app_no; // 승인번호
        protected string quota; // 할부개월
        protected string noinf; // 무이자여부
                                // 포인트
        protected string pnt_issue; // 포인트 서비스사
        protected string add_pnt; // 발생 포인트
        protected string use_pnt; // 사용가능 포인트
        protected string rsv_pnt; // 적립 포인트
        protected string pnt_app_time; // 승인시간
        protected string pnt_app_no; // 승인번호
        protected string pnt_amount; // 적립금액 or 사용금액
                                     // 계좌이체
        protected string bank_name; // 은행명
        protected string bank_code; // 은행코드
                                    // 가상계좌
        protected string bankname; // 입금할 은행
        protected string bankcode; // 입금할 은행코드
        protected string depositor; // 입금할 계좌 예금주
        protected string account; // 입금할 계좌 번호
        protected string va_date; // 가상계좌 입금마감시간
                                  // 휴대폰
        protected string commid; // 통신사 코드
        protected string mobile_no; // 휴대폰 번호
                                    // 상품권
        protected string tk_van_code; // 발급사 코드
        protected string tk_app_no; // 승인 번호
        protected string tk_app_time; // 상품권 승인시간
                                      // 현금 영수증
        protected string cash_yn; // 현금 영수증 등록 여부
        protected string cash_tr_code; // 현금 영수증 발행 구분
        protected string cash_id_info; // 현금 영수증 등록 번호
        protected string cash_authno; // 현금 영수증 승인 번호
        protected string cash_no; // 현금 영수증 거래 번호    

        protected void Page_Load(object sender, EventArgs e)
        {
            /* ============================================================================== */
            /* =   결제 API URL                                                             = */
            /* = -------------------------------------------------------------------------- = */
            target_URL = "https://stg-spl.kcp.co.kr/gw/enc/v1/payment"; // 개발서버
            // target_URL = "https://spl.kcp.co.kr/gw/enc/v1/payment"; // 운영서버
            /* ============================================================================== */
            /* =  요청정보                                                                  = */
            /* = -------------------------------------------------------------------------- = */
            tran_cd = Request.Form["tran_cd"];  // 요청타입
            site_cd = Request.Form["site_cd"];  // 사이트코드
            enc_data = Request.Form["enc_data"]; // 암호화 인증데이터
            enc_info = Request.Form["enc_info"]; // 암호화 인증데이터
            ordr_mony = "1"; // 결제요청금액   ** 1 원은 실제로 업체에서 결제하셔야 될 원 금액을 넣어주셔야 합니다. 결제금액 유효성 검증 **
            /* = -------------------------------------------------------------------------- = */
            use_pay_method = Request.Form["use_pay_method"]; // 결제 방법
            ordr_idxx = Request.Form["ordr_idxx"]; // 주문번호
            cash_yn = Request.Form["cash_yn"]; // 현금 영수증 등록 여부
            cash_tr_code = Request.Form["cash_tr_code"]; //현금 영수증 발행 구분
            cash_id_info = Request.Form["cash_id_info"]; // 현금 영수증 등록 번호

            cash_authno = ""; // 현금 영수증 승인 번호
            cash_no = ""; // 현금 영수증 거래 번호

            req_data = "{\"tran_cd\" : \"" + tran_cd + "\"," +
                                  "\"site_cd\":\"" + site_cd + "\"," +
                                  "\"kcp_cert_info\":\"" + KCP_CERT_INFO + "\"," +
                                  "\"enc_data\":\"" + enc_data + "\"," +
                                  "\"enc_info\":\"" + enc_info + "\"," +
                                  "\"ordr_mony\":\"" + ordr_mony + "\"}";

            // SSL/ TLS 보안 채널을 만들수 없습니다. 오류 발생 시 추가 (프레임워크 버전을 올렸음에도 안될 경우 추가)
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            // API REQ
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(target_URL);
            req.Method = "POST";
            req.ContentType = "application/json";

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
            res_cd = json_data["res_cd"].ToString();
            res_msg = json_data["res_msg"].ToString();

            //정상 응답인 경우
            if (res_cd.Equals("0000"))
            {
                tno = json_data["tno"].ToString();
                res_cd = json_data["res_cd"].ToString();
                res_msg = json_data["res_msg"].ToString();
                // 결제수단 포인트의 경우 pnt_amount로 응답
                if (!use_pay_method.Equals("000100000000"))
                {
                    amount = json_data["amount"].ToString();
                }
                // 카드
                if (use_pay_method.Equals("100000000000"))
                {
                    card_cd = json_data["card_cd"].ToString();
                    card_name = json_data["card_name"].ToString();
                    app_no = json_data["app_no"].ToString();
                    app_time = json_data["app_time"].ToString();
                    noinf = json_data["noinf"].ToString();
                    quota = json_data["quota"].ToString();
                    // 포인트 복합결제
                    pnt_issue = "";
                    if (json_data["pnt_issue"] != null)
                    {
                        pnt_issue = json_data["pnt_issue"].ToString();
                        if (pnt_issue.Equals("SCSK") || pnt_issue.Equals("SCWB"))
                        {
                            pnt_issue = json_data["pnt_issue"].ToString();
                            add_pnt = json_data["add_pnt"].ToString();
                            use_pnt = json_data["use_pnt"].ToString();
                            rsv_pnt = json_data["rsv_pnt"].ToString();
                            pnt_app_time = json_data["pnt_app_time"].ToString();
                            pnt_app_no = json_data["pnt_app_no"].ToString();
                            pnt_amount = json_data["pnt_amount"].ToString();
                            // 현금영수증 발급시 
                            if (cash_yn.Equals("Y"))
                            {
                                cash_authno = json_data["cash_authno"].ToString();
                                cash_no = json_data["cash_no"].ToString();
                            }
                        }
                    }
                }
                // 계좌이체
                else if (use_pay_method.Equals("010000000000"))
                {
                    bank_name = json_data["bank_name"].ToString();
                    bank_code = json_data["bank_code"].ToString();
                    app_time = json_data["app_time"].ToString();

                    // 현금영수증 발급시 
                    if (cash_yn.Equals("Y"))
                    {
                        cash_authno = json_data["cash_authno"].ToString();
                        cash_no = json_data["cash_no"].ToString();
                    }
                }
                // 가상계좌
                else if (use_pay_method.Equals("001000000000"))
                {
                    bankname = json_data["bankname"].ToString();
                    bankcode = json_data["bankcode"].ToString();
                    depositor = json_data["depositor"].ToString();
                    account = json_data["account"].ToString();
                    va_date = json_data["va_date"].ToString();
                    app_time = json_data["app_time"].ToString();

                    // 현금영수증 발급시 
                    if (cash_yn.Equals("Y"))
                    {
                        // 현금영수증 발급 후 처리
                        //cash_authno = json_data["cash_authno"].ToString();
                        //cash_no = json_data["cash_no"].ToString();
                    }
                }
                // 포인트
                else if (use_pay_method.Equals("000100000000"))
                {
                    pnt_issue = json_data["pnt_issue"].ToString();
                    add_pnt = json_data["add_pnt"].ToString();
                    use_pnt = json_data["use_pnt"].ToString();
                    rsv_pnt = json_data["rsv_pnt"].ToString();
                    pnt_app_time = json_data["pnt_app_time"].ToString();
                    pnt_app_no = json_data["pnt_app_no"].ToString();
                    pnt_amount = json_data["pnt_amount"].ToString();
                    // 현금영수증 발급시 
                    if (cash_yn.Equals("Y"))
                    {
                        cash_authno = json_data["cash_authno"].ToString();
                        cash_no = json_data["cash_no"].ToString();
                    }
                }
                // 휴대폰
                else if (use_pay_method.Equals("000010000000"))
                {
                    app_time = json_data["app_time"].ToString();
                    commid = json_data["commid"].ToString();
                    mobile_no = json_data["mobile_no"].ToString();
                }
                // 상품권
                else if (use_pay_method.Equals("000000001000"))
                {
                    tk_van_code = json_data["tk_van_code"].ToString();
                    tk_app_no = json_data["tk_app_no"].ToString();
                    tk_app_time = json_data["tk_app_time"].ToString();
                }
            }
            /* ============================================================================== */
            /* =     승인 결과 DB 처리 실패시 : 자동취소
            /* = -------------------------------------------------------------------------- = */
            /* =     승인 결과를 DB 작업 하는 과정에서 정상적으로 승인된 건에 대해          = */
            /* = DB 작업을 실패하여 DB update 가 완료되지 않은 경우, 자동으로               = */
            /* = 승인 취소 요청을 하는 프로세스가 구성되어 있습니다.                        = */
            /* =                                                                            = */
            /* = DB 작업이 실패 한 경우, bSucc 라는 변수(String)의 값을 "false"             = */
            /* = 로 설정해 주시기 바랍니다. (DB 작업 성공의 경우에는 "false" 이외의         = */
            /* =     값을 설정하시면 됩니다.)                                               = */
            /* = -------------------------------------------------------------------------- = */
            bSucc = "";
            if (res_cd.Equals("0000"))
            {
                if ("false".Equals(bSucc))
                {
                    /* ============================================================================== */
                    /* =    KCP PG-API 가맹점 테스트용 개인키 READ                                         = */
                    /* = -------------------------------------------------------------------------- = */
                    // PKCS#8 PEM READ
                    StreamReader sr = new StreamReader("C:\\...\\dotnet_kcp_api_pay_sample\\certificate\\splPrikeyPKCS8.pem"); // 개인키 경로 ("splPrikeyPKCS8.pem" 은 테스트용 개인키)
                    String privateKeyText = sr.ReadToEnd();

                    // 개인키 비밀번호
                    string privateKeyPass = "changeit"; // 개인키 비밀번호 ("changeit" 은 테스트용 개인키 비밀번호)

                    // 개인키정보 READ
                    StringReader stringReader = new StringReader(privateKeyText);
                    PemReader pemReader = new PemReader(stringReader, new PasswordFinder(privateKeyPass));
                    RsaPrivateCrtKeyParameters keyParams = (RsaPrivateCrtKeyParameters)pemReader.ReadObject();

                    /* ============================================================================== */
                    /* =    kcp_sign_data 생성                                                      = */
                    /* =    - 서명대상데이터 생성규칙                                               = */
                    /* =      : 결제취소(Cancel) : site_cd^tno^mod_type(STSC:전체취소)              = */
                    /* = -------------------------------------------------------------------------- = */
                    String cancel_target_data = site_cd + "^" + tno + "^" + "STSC";

                    byte[] tmpSource = Encoding.ASCII.GetBytes(cancel_target_data);

                    ISigner sign = SignerUtilities.GetSigner(PkcsObjectIdentifiers.Sha256WithRsaEncryption.Id);
                    sign.Init(true, keyParams);
                    sign.BlockUpdate(tmpSource, 0, tmpSource.Length);

                    var sign_data = sign.GenerateSignature();
                    String kcp_sign_data = Convert.ToBase64String(sign_data);

                    /* ============================================================================== */
                    /* =   취소 API URL                                                             = */
                    /* = -------------------------------------------------------------------------- = */
                    target_URL = "https://stg-spl.kcp.co.kr/gw/mod/v1/cancel"; // 개발서버
                    // target_URL = "https://spl.kcp.co.kr/gw/mod/v1/cancel"; // 운영서버
                    /* ============================================================================== */
                    /* =  요청정보                                                                  = */
                    /* = -------------------------------------------------------------------------- = */

                    req_data = "{\"site_cd\" : \"" + site_cd + "\"," +
                                          "\"kcp_cert_info\":\"" + KCP_CERT_INFO + "\"," +
                                          "\"kcp_sign_data\":\"" + kcp_sign_data + "\"," +
                                          "\"tno\":\"" + tno + "\"," +
                                          "\"mod_type\":\"STSC\"," +
                                          "\"mod_desc\":\"가맹점 DB 처리 실패(자동취소)\"}";

                    // SSL/ TLS 보안 채널을 만들수 없습니다. 오류 발생 시 추가 (프레임워크 버전을 올렸음에도 안될 경우 추가)
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    // API REQ
                    req = (HttpWebRequest)WebRequest.Create(target_URL);
                    req.Method = "POST";
                    req.ContentType = "application/json";

                    byte_req = Encoding.UTF8.GetBytes(req_data);
                    req.ContentLength = byte_req.Length;

                    st = req.GetRequestStream();
                    st.Write(byte_req, 0, byte_req.Length);
                    st.Close();

                    // API RES
                    res = (HttpWebResponse)req.GetResponse();
                    st_read = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                    res_data = st_read.ReadToEnd();

                    st_read.Close();
                    res.Close();

                    // RES JSON DATA Parsing
                    json_data = JObject.Parse(res_data);
                    res_cd = json_data["res_cd"].ToString();
                    res_msg = json_data["res_msg"].ToString();
                }
            }
        }
        private class PasswordFinder : IPasswordFinder
        {
            private string password;
            public PasswordFinder(string pwd) 
            {
                password = pwd;
            }
            public char[] GetPassword()
            {
                return password.ToCharArray();
            }   
        }
    }
}