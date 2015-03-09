<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="company.aspx.cs" Inherits="cms.company" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>CMS:Company</title>
    <!--[if lt IE 9]>
      <script src="js/html5shiv.js"></script>
      <script src="js/respond.min.js"></script>
    <![endif]-->
    <meta http-equiv="Default-Style" content="ty" />
    <link rel="stylesheet" href="css/theme/ty.css" title="ty" />
    <link href="css/summernote.css" rel="stylesheet" />
    <link href="css/summernote-bs3.css" rel="stylesheet" />
    <link rel="stylesheet" href="css/dataTables.bootstrap.css" />
    <link rel="stylesheet" href="http://netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.min.css" />
    <style type="text/css">
        .container{padding-top:60px!important;max-width:960px!important;}
        .toCap{text-transform:capitalize;}
        .topbuffer{margin-top:20px;}
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
    
    <div class="row" id="" style="">
        <div class="col-md-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Comapny Details:</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-4">
                            <div class="input-group">
                                <!--span class="input-group-addon">Providers</span-->
                                <select id="ddProviders" class="form-control" style="text-transform: capitalize;">
                                </select>
                                <span class="input-group-btn">
                                    <button id="btnProvider" class="btn btn-primary" type="button">
                                        Select Provider</button>
                                </span>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="input-group">
                                <!--span class="input-group-addon">Company</span-->
                                <select id="ddCompanies" class="form-control" style="text-transform: capitalize;">
                                </select>
                                <span class="input-group-btn">
                                    <button id="btnCompany" class="btn btn-primary" type="button">
                                        Select Company</button>
                                </span>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <button id="btnBranches" class="btn btn-info" type="button">
                                Add/View Branches</button>
                            <%--<button id="btnSaveBranches" class="btn btn-success" type="button" style="display: none;">
                                Save Branches</button>--%>
                        </div>
                        <div class="col-md-2 pull-left">
                            <button id="btnSeoDetail" class="btn btn-info" type="button">
                                SEO Detail</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
        
    <div class="row" id="divCompanyForm" style="display: none;">
        <div class="col-md-12" style="height: 100%;">
            <iframe frameborder="0" seamless="seamless" width="100%" height="100%" scrolling="no"
                id="cms_ul_img" src="new_company.aspx" id="new_company_info" name="new_company_info">
            </iframe>
            <%--<div class="row">
            <div class="col-md-12">
                <button id="btnShowBranches" name="btnSaveInfo" class="btn btn-primary">Show Company Branches</button>
            </div>
        </div>--%>
        </div>
    </div>
    
    <div class="row" id="viewCompanyInfo" style="display: none;">
            <div class="col-md-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        Selected Company Info</div>
                    <div class="panel-body">
                        <div class="table-responsive">
                            <table class="table table-bordered">
                                <tr>
                                    <td colspan="2" class="text-center">
                                        <img id="logo_link" name="logo_link" src="https://s3-ap-southeast-1.amazonaws.com/gds-buspics/dev/1_5217.Jpeg">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Company Name</strong>
                                    </td>
                                    <td>
                                        <h5 name="company_name" id="company_name">
                                        </h5>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Address</strong>
                                    </td>
                                    <td>
                                        <address name="company_address" id="company_address" style="white-space: pre-line;">
                                        </address>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>City</strong>
                                    </td>
                                    <td>
                                        <p name="city_name" id="city_name" class="toCap">
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>State</strong>
                                    </td>
                                    <td>
                                        <p name="state_name" id="state_name">
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Pin No</strong>
                                    </td>
                                    <td>
                                        <strong name="pin_no" id="pin_no"></strong>
                                    </td>
                                </tr>
                                <%--<tr>
                                <td><strong>Email</strong></td>
                                <td><a href="" name="comapany_email" id="comapany_email"></a></td>
                            </tr>
                            <tr>
                                <td><strong>Owner</strong></td>
                                <td><p name="owner_name" id="owner_name"></p></td>
                            </tr>
                            <tr>
                                <td><strong>Owner Contact</strong></td>
                                <td><p name="owner_contact" id="owner_contact"></p></td>
                            </tr>
                            <tr>
                                <td><strong>Manager</strong></td>
                                <td><p name="manager_name" id="manager_name"></p></td>
                            </tr>
                            <tr>
                                <td><strong>Manager Contact</strong></td>
                                <td><p name="manager_contact" id="manager_contact"></p></td>
                            </tr>--%>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
    <div class="row" id="divBranches" style="display: none;">
        <div class="col-md-12">
            <div class="panel panel-primary" id="company_dom">
                <div class="panel-heading">
                    Selected Company:</div>
                <div class="panel-body">
                    <button id="btnAddNewBranch" class="btn btn-info" type="button" data-toggle="modal"
                        data-target="#branchModel">
                        Add New Branch</button>
                    <div class="table-responsive">
                        <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered" id="dtBranches"
                            width="100%">
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div class="row" id="divCompSEODtls" style="display: none;">
            <div class="col-md-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <label id="lblGDSCompanyName" name="lblGDSCompanyName"></label>
                        <input type="hidden" id="hdnCSDId" name="hdnCSDId" /> 
                        <input type="hidden" id="hdnCompId" name="hdnCompId" />
                    </div>
                    <div class="panel-body">
                        <div class="row topbuffer">
                            <div class="col-md-4"><strong>SEO Name</strong></div>
                            <div class="col-md-8">
                                <input type="text" id="txtCSN" name="txtCSN" class="form-control" />
                            </div>
                        </div>
                        <div class="row topbuffer">
                            <div class="col-md-4"><strong>SEO URL</strong></div>
                            <div class="col-md-8">
                                <div class="input-group">
                                    <span class="input-group-addon">http://travelyaari.com/bus-booking/</span>
                                    <input type="text" id="txtSeoUrl" name="txtSeoUrl" class="form-control" />
                                    <span class="input-group-addon">-online.html</span>
                                </div>
                            </div>
                        </div>
                        <div class="row topbuffer">
                            <div class="col-md-12"><div class="summernote"></div></div>
                        </div>
                    </div>
                    <div class="panel-footer" style="padding:2% 2% 6% 91%;">
                        <button type="button" id="btnSaveSEODtl" class="btn btn-primary pull-right form-control-feedback">
                            <span class="glyphicon glyphicon-save">&nbsp;Save changes</span>
                        </button>
                    </div>
                </div>
            </div>
        </div>
    
</div>

<div class="modal fade" id="branchModel" tabindex="-1" role="dialog" aria-labelledby="imageModelLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content" style="width: 820px;">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    &times;</button>
                <h4 class="modal-title" id="imageModelLabel">
                    Save/Update Branch</h4>
            </div>
            <div class="modal-body" style="">
                <div class="row">
                    <div class="col-lg-12">
                        <div class="form-group">
                            <label>
                                Branch Name</label>
                            <div class="row">
                                <div class="col-md-12">
                                    <input class="form-control input-sm" id="txtBName" name="txtBName" placeholder="Branch Name"
                                        required />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label>
                                State</label>
                            <select id="ddState" name="ddState" class="form-control input-sm" required>
                            </select>
                        </div>
                        <div class="form-group">
                            <label>
                                City Name</label>
                            <select id="ddCity" name="ddState" class="form-control input-sm" required>
                            </select>
                        </div>
                    </div>
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label>
                                Address</label>
                            <div class="row">
                                <div class="col-md-12">
                                    <textarea class="form-control" id="taBranch" name="taBranch" rows="5" placeholder="Enter Branch Address"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label>
                                PIN</label>
                            <input class="form-control input-sm" id="txtBPin" name="txtBPin" placeholder="PIN No"
                                required />
                        </div>
                    </div>
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label>
                                Contact No</label>
                            <div class="row">
                                <div class="col-md-12">
                                    <input class="form-control input-sm" id="txtBCont" name="txtBCont" placeholder="Branch Contact No" required />
                                </div>
                            </div>
                            <%--<label>
                                Email</label>
                            <div class="row">
                                <div class="col-md-12">
                                    <input class="form-control input-sm" id="txtBMail" name="txtBMail" placeholder="Branch Email"
                                        required />
                                </div>
                            </div>--%>
                        </div>
                    </div>
                </div>
                <%--<div class="row">
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label>
                                Mangeer Name</label>
                            <input class="form-control input-sm" id="txtBMang" name="txtBMang" placeholder="Manager Name"
                                required />
                        </div>
                    </div>
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label>
                                Manager's Contact</label>
                            <div class="row">
                                <div class="col-lg-12">
                                    <input class="form-control input-sm" id="txtBMCont" name="txtBMCont" placeholder="Manager's  Contact"
                                        required />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>--%>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">
                    Close</button>
                <button type="button" class="btn btn-primary" onclick="SaveBranch();">
                    Save</button>
            </div>
        </div>
    </div>
</div>

<input type="hidden" id="hdnBId" name="hdnBId" value="0" />
<script type="text/javascript" charset="utf-8" src="js/jquery-1.11.0.min.js"></script>
<script type="text/javascript" charset="utf-8" src="js/jquery.dataTables.min.js"></script>
<script type="text/javascript" charset="utf-8" src="js/bootstrap.min.js"></script>
<script type="text/javascript" charset="utf-8" src="js/dataTables.bootstrap.js"></script>
<script type="text/javascript" src="js/summernote.min.js"></script>
<script type="text/javascript" charset="utf-8" src="js/company.js"></script>

</body>
</html>
