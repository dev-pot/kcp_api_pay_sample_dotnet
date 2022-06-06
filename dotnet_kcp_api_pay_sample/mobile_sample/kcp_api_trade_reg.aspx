<%@ Page Language="C#" AutoEventWireup="true" CodeFile="./kcp_api_trade_reg.aspx.cs" Inherits="Payment.Kcp_api_trade_reg" %>
<html>
<head>
    <title>*** NHN KCP API SAMPLE ***</title>
    <meta http-equiv="Content-Type" content="text/html; charset=euc-kr" />
    <meta http-equiv="x-ua-compatible" content="ie=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=yes, target-densitydpi=medium-dpi">  
    <script type="text/javascript">

        function goReq() {
        <%
        // 거래등록 처리 정상
        if ( res_cd.Equals( "0000" ) )
        {
        %>
        alert("거래등록 성공");
        document.form_trade_reg.action = "order_mobile.aspx";
        document.form_trade_reg.submit();
        <%
        }
    
        // 거래등록 처리 실패, 여기(샘플)에서는 trade_reg page로 리턴 합니다.
        else
        {
        %>
        alert("에러 코드 : <%=res_cd%>, 에러 메세지 : <%=res_msg%>");
            location.href = "./trade_reg.html";
        <%
        }
        %>
        }
    </script>
</head>
<body onload="goReq();">
    <div class="wrap">
        <!--  거래등록 form : form_trade_reg -->
        <form name="form_trade_reg" method="post">
            <input type="hidden" name="site_cd"         value="<%=site_cd %>" />  <!-- 사이트 코드 -->
            <input type="hidden" name="ordr_idxx"       value="<%=ordr_idxx %>" /><!-- 주문번호     -->
            <input type="hidden" name="good_mny"        value="<%=good_mny %>" /> <!-- 결제금액     -->
            <input type="hidden" name="good_name"       value="<%=good_name %>" /><!-- 상품명        -->
            <!-- 인증시 필요한 파라미터(변경불가)-->
            <input type="hidden" name="pay_method"      value="<%=pay_method %>" />
            <input type="hidden" name="ActionResult"    value="<%=actionResult %>" />
            <input type="hidden" name="van_code"        value="<%=van_code %>" />
            <!-- 리턴 URL (kcp와 통신후 결제를 요청할 수 있는 암호화 데이터를 전송 받을 가맹점의 주문페이지 URL) -->
            <input type="hidden" name="Ret_URL"         value="<%=Ret_URL %>" />
            <!-- 거래등록 응답 값 -->
            <input type="hidden" name="approvalKey"     value="<%=approvalKey %>" />
            <input type="hidden" name="traceNo"         value="<%=traceNo %>" />
            <input type="hidden" name="PayUrl"          value="<%=PayUrl %>" />
        </form>
    </div>
<!--//wrap-->
</body>
</html>
