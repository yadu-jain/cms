<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="cms.home" %>

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
    <link rel="stylesheet" href="css/dataTables.bootstrap.css" />
    <!--link rel="stylesheet" href="https://datatables.net/release-datatables/extensions/TableTools/css/dataTables.tableTools.css" /-->
    <link rel="stylesheet" href="css/main.css" />
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
                <!--<a class="logo"><img src="img/logo.png"></a>-->
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
    
    <div class="panel panel-primary" id="search-company">
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="input-group">
                        <input type="text" name="srchdComp" id="srchdComp" class="form-control" />
                        <span class="input-group-btn">
                            <button class="btn btn-primary" id="btnSrhComp" type="button">
                            <span class="glyphicon glyphicon-search">&nbsp;</span> Search Company!</button>
                        </span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div class="panel panel-default" id="company_dom">
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="table-responsive">
                        <table cellpadding="0" cellspacing="0" border="0" class="table table-condensed table-bordered table-hover" id="dtCompanies" width="100%"></table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div class="panel panel-primary top-buffer" id="company_lable" style="display:none;">
        <div class="panel-heading">Selected Company:</div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="table-responsive" style="text-transform:capitalize;">
                        <table class="table table-condensed" width="100%" cellpadding="0" cellspacing="0" border="0">
                            <thead>
                                <tr>
                                    <th>Id</th>
                                    <th>Name</th>
                                    <th>City</th>
                                    <th>State</th>
                                    <th>&nbsp;</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td><label id="lblCompanyId"></label></td>
                                    <td><label id="lblCompanyName"></label></td>
                                    <td><label id="lblCompanyCity"></label></td>
                                    <td><label id="lblCompanyState"></label></td>
                                    <td><label><a href="#" onclick="showDTCompany();">Select another Company</a></label></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div class="panel panel-default top-buffer" id="trip_dom" style="display:none;">
        <%--<div class="panel-heading">Trips:</div>--%>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="table-responsive">
                        <table cellpadding="0" cellspacing="0" border="0" class="table table-condensed table-bordered table-hover" id="dtTrips" width="100%"></table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div class="panel panel-primary top-buffer" id="trip_lable" style="display:none;">
        <div class="panel-heading">Selected Trip:</div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="table-responsive" style="text-transform:capitalize;">
                        <table class="table table-condensed" width="100%" cellpadding="0" cellspacing="0" border="0">
                            <thead>
                                <tr>
                                    <th>Trip Id</th>
                                    <th>Route Info</th>
                                    <th>DepartureTime</th>
                                    <th>&nbsp;</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td><label id="lblTripId"></label></td>
                                    <td><label id="lblRouteInfo"></label></td>
                                    <td><label id="lblDepartureTime"></label></td>
                                    <td style="min-width:150px;"><label><a href="#" onclick="showDTTrip();">Select another Trip</a></label></td>
                                </tr>
                                <tr>
                                    <td colspan="3"><label class="pull-right"><a id="tyActiveBus" href="" target="_blank" class="">Currently Serving Bus Pics in TY</a></label></td>
                                    <td>&nbsp;</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="panel panel-primary" id="featues_upload" style="display:none;">
        <div class="panel-heading">Features:</div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="row amn-cb-wrap">
                        <div id="amnlst"></div>
                    </div>
                    <%--<div class="row">
                        <div class="col-md-3">
                            <div class="checkbox"><label><input type="checkbox" name="cbFeature[]" value="cb_tv" />TV</label></div>
                        </div>
                        <div class="col-md-3">
                            <div class="checkbox"><label><input type="checkbox" name="cbFeature[]" value="cb_wf" />WiFi</label></div>
                        </div>
                        <div class="col-md-3">
                            <div class="checkbox"><label><input type="checkbox" name="cbFeature[]" value="cb_tl" />Toilet</label></div>
                        </div>
                        <div class="col-md-3">
                            <div class="checkbox"><label><input type="checkbox" name="cbFeature[]" value="cb_sk" />Snacks</label></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <div class="checkbox"><label><input type="checkbox" name="cbFeature[]" value="cb_bk" />Blanket</label></div>
                        </div>
                        <div class="col-md-3">
                            <div class="checkbox"><label><input type="checkbox" name="cbFeature[]" value="cb_wb" />Water Bottle</label></div>
                        </div>
                        <div class="col-md-3">
                            <div class="checkbox"><label><input type="checkbox" name="cbFeature[]" value="cb_cp" />Charging Point</label></div>
                        </div>
                        <div class="col-md-3">
                            <div class="checkbox"><label><input type="checkbox" name="cbFeature[]" value="cb_pt" />Personalize TV</label></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <div class="checkbox"><label><input type="checkbox" name="cbFeature[]" value="cb_bk" />Blanket</label></div>
                        </div>
                        <div class="col-md-3">
                            
                        </div>
                        <div class="col-md-3">
                            
                        </div>
                        <div class="col-md-3">
                            
                        </div>
                    </div>--%>
                </div>
            </div>
        </div>
        <div class="panel-footer">
            <div class="row">
                <div class="col-md-10" style="height:50px;">
                    <div id="feature_succes" style="display:none;">
                        <div class="alert alert-success"><strong>Well done!</strong> You updated features sucessfully...</div>
                    </div>
                </div>
                <div class="col-md-2">
                    <button id="save_features" type="button" class="btn btn-primary navbar-btn">
                    <span class="glyphicon glyphicon-save"></span>
                    Save Feature</button>
                </div>
            </div>
        </div>
    </div>
    
    <div class="panel panel-default" id="trips_buses" style="display:none;">
        <div class="panel-heading">Bus List::</div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="table-responsive">
                        <table cellpadding="0" cellspacing="0" border="0" class="table table-condensed table-bordered table-hover" id="tabBuses" width="100%">
                        </table>
                    </div>
                </div>
            </div>
            <div class="row topbuffer">
                <div class="col-md-12">
                    <div class="input-group">
                        <select id="ddCompBuses" class="form-control" style="text-transform: capitalize;">
                        </select>
                        <span class="input-group-btn">
                            <button id="btnMapBus" class="btn btn-primary" type="button">
                                Map Bus No to Trip</button>
                        </span>
                    </div>
                </div>
            </div>
            <div class="row topbuffer">
                <div class="col-md-12">
                    <div class="input-group">
                        <input type="text" id="txtNewBusNo" class="form-control" />
                        <span class="input-group-btn">
                            <button id="btnAddBus" class="btn btn-primary" type="button">
                                Add New Bus No to Trip</button>
                        </span>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div id="added_bus" style="display:none;">
                        <div class="alert alert-success"><strong>Well done!</strong> You added bus in this trip sucessfully...</div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
</div>
<script type="text/javascript" charset="utf-8" src="js/jquery-1.11.0.min.js"></script>
<script type="text/javascript" charset="utf-8" src="js/jquery.dataTables.min.js"></script>
<script type="text/javascript" charset="utf-8" src="js/bootstrap.min.js"></script>
<script type="text/javascript" charset="utf-8" src="js/dataTables.bootstrap.js" ></script>
<!--script type="text/javascript" language="javascript" src="http://datatables.net/release-datatables/extensions/TableTools/js/dataTables.tableTools.js"></script-->

<script type="text/javascript" charset="utf-8" src="js/home.js"></script>    
</body>
</html>
