<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="buses.aspx.cs" Inherits="cms.home" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>CMS</title>
    <!--[if lt IE 9]>
      <script src="js/html5shiv.js"></script>
      <script src="js/respond.min.js"></script>
    <![endif]-->
    <link rel="stylesheet" href="css/bootstrap.min.css" />
    <meta http-equiv="Default-Style" content="ty" />
    <link rel="stylesheet" href="css/theme/ty.css" title="ty" />
    <%--<link rel="stylesheet" href="css/theme/blue.css" title="blue" />
    <link rel="stylesheet" href="css/theme/yeti.css" title="yeti" />
    <link rel="stylesheet" href="css/theme/slate.css" title="slate" />
    <link rel="stylesheet" href="css/theme/cosmo.css" title="cosmo" />
    <link rel="stylesheet" href="css/theme/lumen.css" title="lumen" />
    <link rel="stylesheet" href="css/theme/amelia.css" title="amelia" />
    <link rel="stylesheet" href="css/theme/united.css" title="united" />
    <link rel="stylesheet" href="css/theme/cyborg.css" title="cyborg" />
    <link rel="stylesheet" href="css/theme/darkly.css" title="darkly" />
    <link rel="stylesheet" href="css/theme/flatly.css" title="flatly" />
    <link rel="stylesheet" href="css/theme/journal.css" title="journal" />
    <link rel="stylesheet" href="css/theme/simplex.css" title="simplex" />
    <link rel="stylesheet" href="css/theme/cerulean.css" title="cerulean" />
    <link rel="stylesheet" href="css/theme/readable.css" title="readable" />
    <link rel="stylesheet" href="css/theme/spacelab.css" title="spacelab" />
    <link rel="stylesheet" href="css/theme/superhero.css" title="superhero" />--%>
    <link rel="Stylesheet" href="css/dataTables.bootstrap.css" />
    <style type="text/css">
        .container{padding-top:60px!important;max-width:960px!important;}
        .col-md-3 {padding:12px;border:4px solid #fff}
        .thumb-align{max-width:192px;max-height:144px;min-width:192px;min-height:144px;display:block;text-align:center;vertical-align:middle;margin-bottom:4px;}
    </style>
</head>
<body>
<div class="container">
    <nav class="navbar navbar-inverse navbar-fixed-top" role="navigation">
        <div class="container-fluid">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-cms-navbar-collapse">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="home.aspx">CMS</a>
            </div>
            <div class="collapse navbar-collapse" id="bs-cms-navbar-collapse">
                <ul class="nav navbar-nav navbar-right">
                    <li><a href="home.aspx">Routes</a></li>
                    <li><a href="company.aspx">Companies</a></li>
                    <li><a href="Default.aspx">Logout</a></li>
                </ul>
            </div>
        </div>
    </nav>
    
    <div class="panel panel-primary" id="gallery">
        <div class="panel-heading"><h4><label id="lblBusNo"></label></h4></div>
        <div class="panel-body">
            <div id="gallery-thumb"></div>
        </div>
        <div class="panel-footer" style="text-align:center;">
            <button type="button" id="btnActiveBus" class="btn btn-primary" data-toggle="modal">
                <span class="glyphicon glyphicon-flash">&nbsp;Active This Bus</span>
            </button>
            <button type="button" id="btnAddImage" class="btn btn-primary" data-toggle="modal" data-target="#imgUlMdl"> 
                <span class="glyphicon glyphicon-plus">&nbsp;Upload New Image</span>
            </button>
            <button type="button" id="btnDelete" class="btn btn-primary">
                <span class="glyphicon glyphicon-trash">&nbsp;Delete Selected Images</span>
            </button>
            <button type="button" id="btnGalSave" class="btn btn-primary">
                <span class="glyphicon glyphicon-save">&nbsp;Save changes</span>
            </button>
        </div>
    </div>
</div>

<div class="modal fade" id="imgUlMdl" tabindex="-1" role="dialog" aria-labelledby="imgUlMdlLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content" style="width:820px;">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title" id="imageModelLabel">Upload Images for Buses</h4>
            </div>
            <div class="modal-body" style="height:720px;">
                <iframe frameborder="0" seamless="seamless" width="100%" height="100%" scrolling="no" 
                    id="cms_ul_img" src="cms_upload_bus_pix.aspx">
                </iframe>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript" charset="utf-8" src="js/jquery-1.11.0.min.js"></script>
<script type="text/javascript" charset="utf-8" src="js/jquery.dataTables.min.js"></script>
<script type="text/javascript" charset="utf-8" src="js/bootstrap.min.js"></script>
<script type="text/javascript" charset="utf-8" src="js/dataTables.bootstrap.js" ></script>
<script type="text/javascript" charset="utf-8" src="js/buses.js"></script>    
</body>
</html>
