<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="cms._Default" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="css/bootstrap.min.css" />
    <meta http-equiv="Default-Style" content="ty" />
    <link rel="stylesheet" href="css/theme/ty.css" title="ty" />
    <style type="text/css">
        #fullBg{z-index:-1;height:100%;width:100%;position:absolute;top:0px;background:url("img/login.jpg") no-repeat center center fixed;background-size:cover;}
        .form-signin{max-width:280px;margin:60px auto 10px;}
        .form-signin .form-signin-heading{text-align:center;font-weight:bold;text-shadow:0px 1px 2px #111;color:#fff;margin-bottom:20px;}
        .form-signin .checkbox{font-weight:normal;}
        .form-signin .form-control{position:relative;font-size:16px;height:auto;padding:10px;-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box;}
        .form-signin input[type="text"]{margin-bottom:-1px;border-top:1px solid #000;border-right:1px solid #000;border-left:1px solid #000;border-radius:6px 6px 0px 0px;}
        .form-signin input[type="text"]:focus{box-shadow:inset 0 1px 1px rgba(0, 0, 0, 0.075);}
        .form-signin input[type="password"]{z-index:2;margin-bottom:20px;border-top:none;border-bottom:1px solid #000;border-right:1px solid #000;border-left:1px solid #000;border-radius:0px 0px 6px 6px;box-shadow:inset 0 1px 1px rgba(0, 0, 0, 0.075), 0px 1px 0px 0px rgba(255, 255, 255, 0.5);}
        .form-signin input[type="password"]:focus{box-shadow:inset 0 1px 1px rgba(0, 0, 0, 0.075), 0px 1px 0px 0px rgba(255, 255, 255, 0.5);}
        .form-signin .btn{font-weight:bold;text-shadow:0px -1px 0px rgba(0, 0, 0, 0.2);}
        p.sign-up{text-shadow:0px 1px 2px #222;color:#fff;font-size:12px;}
    </style>
</head>
<body>
    <div id="fullBg">
        <div class="container">
            <form class="form-signin" id="loginform" role="form" action="" runat="server">
            <h1 class="form-signin-heading">Please Login</h1>
            <input type="text" class="form-control" name="username" id="username" runat="server" placeholder="User Name" value="yadu" required />
            <input type="password" class="form-control" name="password" id="password" placeholder="Password" value="1" required />
            <button class="btn btn-lg btn-primary btn-block" onserverclick="btnLogin_Click" id="btnLogin" runat="server"> Log in</button>
            <div id="divError" runat="server" visible="false"><div class="alert alert-danger"><label id="lblError" runat="server"></label></div></div>
            </form>
            <%--<p class="text-center sign-up"><strong>Sign up</strong> for a new account</p>--%>
        </div>
    </div>

    <script type="text/ecmascript" src="js/jquery-1.11.0.min.js"></script>
    <script type="text/ecmascript" src="js/bootstrap.min.js"></script>
    <script type="text/ecmascript" src="js/jquery.validate.js"></script>
    <script type="text/ecmascript" src="js/jquery-validate.bootstrap-tooltip.js"></script>
    <script type="text/javascript">
        $(function() {
            /*$('#loginform').validate({
                rules: {
                    username: { required: true },
                    password: { required: true }
                },
                messages: {
                    username: { required: '[Required]' },
                    password: { required: '[Required]' }
                },
                tooltip_options: {
                    username: { placement: 'top' },
                    password: { placement: 'top' }
                }
            });*/

        });
    </script>

</body>
</html>
