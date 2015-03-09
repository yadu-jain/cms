<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="seo_operator_content.aspx.cs" Inherits="cms.seo_operator_content" %>

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
    <link rel="Stylesheet" href="css/dataTables.bootstrap.css" />
    <link href="css/summernote.css" rel="stylesheet" />
    <link href="css/summernote-bs3.css" rel="stylesheet" />
    
    <style type="text/css">
        .container{padding-top:60px!important;max-width:960px!important;}
        .toCap{text-transform:capitalize;}
        .topbuffer{margin-top:20px;}
    </style>
</head>
<body>
<div class="container">
    <div class="panel panel-default" id="company_dom">
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="table-responsive">
                        <table cellpadding="0" cellspacing="0" border="0" class="table table-condensed table-bordered table-hover" id="dtSEOCompanies" width="100%"></table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--<div class="panel panel-primary">
        <div class="panel-heading">About Us:</div>
        <div class="panel-body">
            <div class="summernote"></div>
        </div>
    </div>--%>
</div>
<script type="text/javascript" charset="utf-8" src="js/jquery-1.11.0.min.js"></script>
<script type="text/javascript" charset="utf-8" src="js/jquery.dataTables.min.js"></script>
<script type="text/javascript" charset="utf-8" src="js/bootstrap.min.js"></script>
<script type="text/javascript" charset="utf-8" src="js/dataTables.bootstrap.js" ></script>
<%--<script type="text/javascript" src="js/summernote.min.js"></script>--%>
<script type="text/javascript">
    var createSeoCompanyDataTable = function() {
    $('#dtSEOCompanies').dataTable({
        "sDom": "<'row'<'col-xs-9'T><'col-xs-3'f>r>t<'row'<'col-xs-6'i><'col-xs-6'p>>",
        "bSortClasses": false,
        "sAjaxDataProp": "",
        "sAjaxSource": "handler/seo.ashx?m=gsc",
        "oLanguage": {
            /*"sLengthMenu": 'Display <select>'+
            '<option value="10">10</option>'+
            '<option value="20">20</option>'+
            '<option value="30">30</option>'+
            '<option value="40">40</option>'+
            '<option value="50">50</option>'+
            '<option value="-1">All</option>'+
            '</select> records'*/
            "sLengthMenu": "Display _MENU_ records"
        },
        "iDisplayLength": 30,
        //"lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
        "bDestroy": true,
        "aaSorting": [[2, 'asc']],
        "aoColumns": [
                    { "mData": "Id",            "sTitle": "Id", "bVisible": false },
                    { "mData": "CompanyId", "sTitle": "GDS Company Id", "sWidth": "04%" },
                    { "mData": "CompanyName", "sTitle": "GDS Company Name", "sWidth": "16%" },
                    { "mData": "SEOCompanyName", "sTitle": "SEO Company Name", "sWidth": "16%" },
                    { "mData": "SeoUrl", "sTitle": "SEO URL", "sWidth": "20%" },
                    { "mData": "StaticWriteup", "sTitle": "Static Writeup", "sWidth": "44%","bAutoWidth": false }
                ],
        "fnRowCallback": function(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(nRow).on('click', function() {
                createTripDataTable(aData);
            });
        }
    });
    $('#dtSEOCompanies_filter input').addClass('form-control');
    };
    $(document).ready(function() {
        //$('.summernote').summernote();
        /*$('.summernote').summernote({
            height: 150,
            toolbar: [],
            //onpaste: function(e) {
                //$('.summernote').code().replace(/(<([^>]+)>)/ig, "");
            //}
        });*/
        createSeoCompanyDataTable(0);
    });
</script>
</body>
</html>
