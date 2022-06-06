<%@ Page Language="C#" AutoEventWireup="true" CodeFile="./kcp_api_pay.aspx.cs" Inherits="Payment.Kcp_api_pay" %>
<html>
<head>
    <title>*** NHN KCP API SAMPLE ***</title>
    <meta http-equiv="Content-Type" content="text/html; charset=euc-kr" />
    <meta http-equiv="x-ua-compatible" content="ie=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=yes, target-densitydpi=medium-dpi">
    <link href="static/css/style.css" rel="stylesheet" type="text/css"/>
    <script type="text/javascript">
        /* 신용카드 영수증 */ 
        /* 실결제시 : "https://admin8.kcp.co.kr/assist/bill.BillActionNew.do?cmd=card_bill&tno=" */ 
        /* 테스트시 : "https://testadmin8.kcp.co.kr/assist/bill.BillActionNew.do?cmd=card_bill&tno=" */ 
         function receiptView( tno, ordr_idxx, amount ) 
        {
            receiptWin = "https://testadmin8.kcp.co.kr/assist/bill.BillActionNew.do?cmd=card_bill&tno=";
            receiptWin += tno + "&";
            receiptWin += "order_no=" + ordr_idxx + "&"; 
            receiptWin += "trade_mony=" + amount ;
    
            window.open(receiptWin, "", "width=455, height=815"); 
        }
    
        /* 현금 영수증 */ 
        /* 실결제시 : "https://admin8.kcp.co.kr/assist/bill.BillActionNew.do" */ 
        /* 테스트시 : "https://testadmin8.kcp.co.kr/assist/bill.BillActionNew.do" */   
        function receiptView2( cash_no, ordr_idxx, amount ) 
        {
            receiptWin2 = "https://testadmin8.kcp.co.kr/assist/bill.BillActionNew.do?cmd=cash_bill&cash_no=";
            receiptWin2 += cash_no + "&";             
            receiptWin2 += "order_id="     + ordr_idxx + "&";
            receiptWin2 += "trade_mony="  + amount ;
    
            window.open(receiptWin2, "", "width=370, height=625"); 
        }
    
        /* 가상 계좌 모의입금 페이지 호출 */
        /* 테스트시에만 사용가능 */
        /* 실결제시 해당 스크립트 주석처리 */
        function receiptView3() 
        {
            receiptWin3 = "http://devadmin.kcp.co.kr/Modules/Noti/TEST_Vcnt_Noti.jsp"; 
            window.open(receiptWin3, "", "width=520, height=300"); 
        }
    </script>
</head>
<body oncontextmenu="return false;">
    <div class="wrap">
        <!-- header -->
        <div class="header">
            <a href="index.html" class="btn-back"><span>뒤로가기</span></a>
            <h1 class="title">TEST SAMPLE</h1>
        </div>
        <!-- //header -->
        <!-- contents -->
        <div id="skipCont" class="contents">
            <h2 class="title-type-3">요청  DATA</h2>
            <ul class="list-type-1">
                <li>
                    <div class="left">
                        <p class="title"></p>
                    </div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-3">
                            <textarea style="height:200px; width:450px" readonly><%=req_data%></textarea>
                        </div>
                    </div>
                </li>
            </ul>
            <h2 class="title-type-3">응답  DATA </h2>
            <ul class="list-type-1">
                <li>
                    <div class="left">
                        <p class="title"></p>
                    </div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-3">
                            <textarea style="height:200px; width:450px" readonly><%=res_data%></textarea>
                        </div>
                    </div>
                </li>
            </ul>
            <h2 class="title-type-3">처리 결과 </h2>
            <ul class="list-type-1">
                <li>
                    <div class="left"><p class="title">결과코드</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=res_cd %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">결과메세지</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=res_msg %><br/>
                        </div>
                    </div>
                </li>
                 <% 
                 if (bSucc == "false") 
                 {
                 %>
                <li>
                    <div class="left"><p class="title">결과 상세 메세지</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%
                            if ( "0000".Equals(res_cd) )
                            {
                            %>
                            결제는 정상적으로 이루어졌지만 쇼핑몰에서 결제 결과를 처리하는 중 오류가 발생하여 자동으로 취소 처리 되었습니다.
                            <% 
                            }
                            else 
                            {
                            %> 
                            결제는 정상적으로 이루어졌지만 쇼핑몰에서 결제 결과를 처리하는 중 오류가 발생하여 자동으로 취소 요청 하였으나, 취소가 실패 되었습니다.
                            <%
                            }
                            %>
                        </div>
                    </div>
                </li>
                <%
                 }
                %>
            </ul>
            <%
            if ( "0000".Equals(res_cd) && "".Equals(bSucc) )
            {
            %>
            <h2 class="title-type-3">공통 </h2>
            <ul class="list-type-1">
                <li>
                    <div class="left"><p class="title">KCP 거래번호</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=tno %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">결제금액</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=amount %>
                        </div>
                    </div>
                </li>
            </ul>
            <%
                // 신용카드 결제 결과 출력
                if ( use_pay_method.Equals("100000000000") )
                {
            %>
            <h2 class="title-type-3">카드 </h2>
            <ul class="list-type-1">
                <li>
                    <div class="left"><p class="title">카드</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=card_name %>(<%=card_cd %>)
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">승인번호</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=app_no %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">할부개월</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=quota %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">무이자여부</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=noinf %>
                        </div>
                    </div>
                </li>
                <%
                    // 복합결제(포인트+신용카드) 승인 결과 처리
                    if ( pnt_issue.Equals("SCSK") || pnt_issue.Equals( "SCWB" ) )
                    {
                %>
                <li>
                    <div class="left"><p class="title">포인트사</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=pnt_issue %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">포인트 승인시간</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=pnt_app_time %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">포인트 승인번호</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=pnt_app_no %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">적립금액  or 사용금액</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=pnt_amount %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">발생 포인트</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=add_pnt %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">사용가능 포인트</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=use_pnt %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">총 누적 포인트</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=rsv_pnt %>
                        </div>
                    </div>
                </li>
                <!-- 포인트 현금영수증 출력 -->
                <% 
                        if (cash_yn.Equals("Y")) 
                        {
                %>
                <li>
                    <div class="left"><p class="title">현금영수증 확인</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <a href="javascript:receiptView2('<%= cash_no %>', '<%= ordr_idxx %>', '<%= pnt_amount %>' )"><span style="color:blue">현금영수증을  확인합니다.</span></a>
                        </div>
                    </div>
                </li>
                <% 
                        }
                    }
                %>
                <!-- 신용카드 영수증 확인 -->
                <li>
                    <div class="left"><p class="title">영수증 확인</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <a href="javascript:receiptView('<%=tno%>','<%=ordr_idxx%>','<%=amount%>')"><span style="color:blue">영수증을 확인합니다.</span></a>
                        </div>
                    </div>
                </li>
            </ul>
            <%
                }
                // 계좌이체 결과 출력
                else if ( use_pay_method.Equals("010000000000") )
                {
            %>
            <h2 class="title-type-3">계좌이체 </h2>
            <ul class="list-type-1">
                <li>
                    <div class="left"><p class="title">계좌이체시간</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=app_time %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">이체은행</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=bank_name %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">은행코드</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=bank_code %>
                        </div>
                    </div>
                </li>
            </ul>
            <%
                }
                // 가상계좌 결과 출력
                else if ( use_pay_method.Equals("001000000000") )
                {
            %>
            <h2 class="title-type-3">가상계좌 </h2>
            <ul class="list-type-1">
                <li>
                    <div class="left"><p class="title">가상계좌 채번시간</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=app_time %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">채번은행</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=bankname %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">채번은행코드</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=bankcode %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">가상계좌번호</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=account %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">입금할 계좌 입금주</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=depositor %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">가상계좌 입금마감일자</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=va_date %>
                        </div>
                    </div>
                </li>
                <!-- 모의 입금 -->
                <li>
                    <div class="left"><p class="title">가상계좌 모의입금<br/>(테스트시 사용)</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <a href="javascript:receiptView3()"><span style="color:blue">모의입금 페이지로 이동합니다.</span></a>
                        </div>
                    </div>
                </li>
            </ul>
            <%
                }
                // 포인트 결과 출력
                else if ( use_pay_method.Equals("000100000000") )
                {
            %>
            <h2 class="title-type-3">포인트 </h2>
            <ul class="list-type-1">
                <li>
                    <div class="left"><p class="title">포인트사</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=pnt_issue %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">포인트 승인시간</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=pnt_app_time %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">포인트 승인번호</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=pnt_app_no %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">적립금액 or 사용금액</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=pnt_amount %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">발생 포인트</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=add_pnt %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">사용가능 포인트</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=use_pnt %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">총 누적 포인트</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=rsv_pnt %>
                        </div>
                    </div>
                </li>
            </ul>
            <%
                }
                // 휴대폰 결과 출력
                else if ( use_pay_method.Equals("000010000000") )
                {
            %>
            <h2 class="title-type-3">휴대폰 </h2>
            <ul class="list-type-1">
                <li>
                    <div class="left"><p class="title">휴대폰 결제시간</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=app_time %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">통신사 코드</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=commid %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">휴대폰 번호</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=mobile_no %>
                        </div>
                    </div>
                </li>
            </ul>
            <%
                }
                // 상품권 결과 출력
                else if ( use_pay_method.Equals("000000001000") )
                {
            %>
            <h2 class="title-type-3">상품권 </h2>
            <ul class="list-type-1">
                <li>
                    <div class="left"><p class="title">발급사 코드</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=tk_van_code %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">승인 시간</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=tk_app_time %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">승인 번호</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=tk_app_no %>
                        </div>
                    </div>
                </li>
            </ul>
            
            <%
                }
                // 현금영수증 정보 출력
                if( !"".Equals ( cash_yn ) )
                {
                    // 결제수단 가상계좌, 계좌이체, 포인트
                    if ( "010000000000".Equals ( use_pay_method ) | "001000000000".Equals ( use_pay_method ) | "000100000000".Equals ( use_pay_method ) )
                    {
            %>
            <h2 class="title-type-3">현금영수증 </h2>
            <ul class="list-type-1">
                <li>
                    <div class="left"><p class="title">현금영수증 등록여부</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=cash_yn %>
                        </div>
                    </div>
                </li>
                <% 
                        //현금영수증이 등록된 경우 승인번호 값이 존재
                        if( !"".Equals ( cash_authno ) )
                        {
                %>
                <li>
                    <div class="left"><p class="title">현금영수증 승인번호</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=cash_authno %>
                        </div>
                    </div>
                </li>
                <li>
                    <div class="left"><p class="title">현금영수증 거래번호</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <%=cash_no %>
                        </div>
                    </div>
                </li>
                <!-- 현금영수증 출력 -->
                <li>
                    <div class="left"><p class="title">현금영수증 확인</p></div>
                    <div class="right">
                        <div class="ipt-type-1 pc-wd-2">
                            <% 
                            // 결제수단 포인트
                            if ("000100000000".Equals ( use_pay_method ))
                            {
                            %>
                            <a href="javascript:receiptView2('<%= cash_no %>', '<%= ordr_idxx %>', '<%= pnt_amount %>' )"><span style="color:blue">현금영수증을  확인합니다.</span></a>
                            <%
                            }
                            else 
                            {
                            %>
                            <a href="javascript:receiptView2('<%= cash_no %>', '<%= ordr_idxx %>', '<%= amount %>' )"><span style="color:blue">현금영수증을  확인합니다.</span></a>
                            <%
                            }
                            %>
                        </div>
                    </div>
                </li>
            </ul>
            <%
                        }
                    }
                }
            }
            
            %>
            
            <ul class="list-btn-2">
                <li class="pc-only-show"><a href="index.html" class="btn-type-3 pc-wd-2">처음으로</a></li>
            </ul>
        </div>
        <div class="grid-footer">
            <div class="inner">
                <!-- footer -->
                <div class="footer">
                    ⓒ NHN KCP Corp.
                </div>
                <!-- //footer -->
            </div>
        </div>
    </div>
    <!--//wrap-->
</body>
</html>
