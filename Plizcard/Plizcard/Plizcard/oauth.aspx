<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="oauth.aspx.cs" Inherits="PlizCard.oauth" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="https://yastatic.net/jquery/2.2.3/jquery.min.js"></script>
    <script>window.jQuery || document.write('<script src="js/jquery.min.js">\x3C/script>')</script>
<script src="/Scripts/oksdk.js"></script>

<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>



      <script type="text/javascript">
          $(function () {

              var srch = window.location.search;

              if (srch.startsWith("?code=")) {
                  console.log('vk');
                  srch = srch.replace("?code=", "web");

                  if (window.opener.$("#addvkvalue").length == 1) {
                      window.opener.$("#addvkvalue").val(srch).trigger("change");
                  } else {
                      window.opener.$("#login-modal-input-vk").val(srch);
                      window.opener.$("#logintosite").submit();
                  }

                 // window.close();
              }

              if (srch.startsWith("?fb=")) {
                  console.log('fb');
                  var ar = Array();
                  var answ = {};
                  var ar = window.location.hash.replace("#", "").split("&");
                  for (var i = 0; i < ar.length ; i++) {
                      var t = ar[i];
                      var tt = t.split("=");
                      if (tt[0] == "code") {
                          srch = "web" + tt[1];
                          if (window.opener.$("#addfbvalue").length == 1) {
                              window.opener.$("#addfbvalue").val(srch).trigger("change");
                          } else {
                              window.opener.$("#login-modal-input-fb").val(srch);
                              window.opener.$("#logintosite").submit();
                          }
                      }
                  }
              }

              if (srch.startsWith("?ok=")) {
                  console.log('ok');
                  var ar = Array();
                  var answ = {};
                  var ar = window.location.search.replace("?", "").split("&");
                  for (var i = 0; i < ar.length ; i++) {
                      var t = ar[i];
                      var tt = t.split("=");
                      if (tt[0] == "code") {
                          srch = "web" + tt[1];
                          if (window.opener.$("#addokvalue").length == 1) {
                              window.opener.$("#addokvalue").val(srch).trigger("change");
                          } else {
                              window.opener.$("#login-modal-input-ok").val(srch);
                              window.opener.$("#logintosite").submit();
                          }
                      }
                  }
              }

              window.close();
        });
    </script>


</head>
<body>

</body>
</html>
