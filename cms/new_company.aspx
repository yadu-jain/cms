<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="new_company.aspx.cs" Inherits="cms.new_company" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Upload Company Logo</title>
    
    <link rel="stylesheet" href="css/jq-ui.css" />
    <link rel="stylesheet" href="css/jquery.fileupload-ui.css" />
    <%--<link rel="stylesheet" href="//iamgds.com/cms/css/style.css" />--%>
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
</head>
<body>
<div class="container">
    <div class="panel panel-primary">
        <div class="panel-heading">Company HO Information:</div>
        <div class="panel-body">
            <div id="fileupload" style="min-height: 580px; max-height: 580px;">
                <form action="cms.ashx" method="POST" enctype="multipart/form-data" name="form1" id="form1">
                <input type="hidden" id="pt" name="pt" value="logo" />
                <div class="row" style="height:120px">
                    <div class="col-md-3 pull-left" style="width: 30%;vertical-align:middle;">
                        <strong class="pull-right">Head Office Address</strong></div>
                    <div class="col-md-9 pull-right" style="width: 70%">
                        <textarea class="form-control" id="taAddress" name="taAddress" rows="5" placeholder="Enter Address separated by line."></textarea>
                    </div>
                </div>
                <div class="row" style="margin:1%;">
                    <div class="col-md-6 pull-left" style="width:50%;">
                        <select id="ddState" name="ddState" class="form-control ttc">
                            <option value="-1">Select State</option>
                        </select>
                    </div>
                    <div class="col-md-6 pull-right" style="width:50%;">
                        <select id="ddCity" name="ddCity" class="form-control ttc">
                            <option value="-1">Select City</option>
                        </select>
                    </div>
                </div>
                <div class="row" style="margin:1%;">
                    <div class="col-md-2">&nbsp;</div>
                    <div class="col-md-8" style="width:60%;margin:0 auto;">
                        <input id="txtPinNo" name="txtPinNo" type="text" placeholder="Enter Pin No" class="form-control input-md">
                    </div>
                    <div class="col-md-2">&nbsp;</div>
                </div>
                
                <div class="row" style="margin:1%;">
                    <div class="col-md-4" style="width:69%; margin: 0 auto;">
                        <div class="fileupload-buttonbar" style="padding:1%;">
                            <label class="fileinput-button">
                                <span>Add Company's Logo...</span>
                                <input id="file" type="file" name="file" />
                            </label>
                            <button type="submit" class="start">
                                Save Company Details </button>
                        </div>
                    </div>
                </div>
                <%--<div class="row" style="margin:1%;">
                    <div class="col-md-12">
                        <button type="submit" id="btnSaveInfo" name="btnSaveInfo" class="btn btn-primary">
                            Save Company Info</button>
                    </div>
                </div>--%>
                </form>
                <hr />
                <div class="row" style="margin:1%;border:0;">
                    <div class="col-md-12">
                        <div class="fileupload-content">
                            <table class="files" style="margin:0 auto;"></table>
                            <div class="fileupload-progressbar"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
    <input type="hidden" id="hdnACS" name="hdnACS" />

    <script id="template-upload" type="text/x-jquery-tmpl">
    <tr class="template-upload{{if error}} ui-state-error{{/if}}">
        <td class="preview"></td>
        <td class="name">${name}</td>
        <td class="size">${sizef}</td>
        {{if error}}
            <td class="error" colspan="2">Error:
                {{if error === 'maxFileSize'}}File is too big
                {{else error === 'minFileSize'}}File is too small
                {{else error === 'acceptFileTypes'}}Filetype not allowed
                {{else error === 'maxNumberOfFiles'}}Max number of files exceeded
                {{else}}${error}
                {{/if}}
            </td>
        {{else}}
            <td class="progress"><div></div></td>
            <td class="start"><button>Start</button></td>
        {{/if}}
        <td class="cancel"><button>Cancel</button></td>
    </tr>
    </script>

    <script id="template-download" type="text/x-jquery-tmpl">
    <tr class="template-download{{if error}} ui-state-error{{/if}}">
        {{if error}}
            <td></td>
            <td class="name">${namefdsa}</td>
            <td class="size">${sizef}</td>
            <td class="error" colspan="2">Error:
                {{if error === 1}}File exceeds upload_max_filesize (php.ini directive)
                {{else error === 2}}File exceeds MAX_FILE_SIZE (HTML form directive)
                {{else error === 3}}File was only partially uploaded
                {{else error === 4}}No File was uploaded
                {{else error === 5}}Missing a temporary folder
                {{else error === 6}}Failed to write file to disk
                {{else error === 7}}File upload stopped by extension
                {{else error === 'maxFileSize'}}File is too big
                {{else error === 'minFileSize'}}File is too small
                {{else error === 'acceptFileTypes'}}Filetype not allowed
                {{else error === 'maxNumberOfFiles'}}Max number of files exceeded
                {{else error === 'uploadedBytes'}}Uploaded bytes exceed file size
                {{else error === 'emptyResult'}}Empty file upload result
                {{else}}${error}
                {{/if}}
            </td>
        {{else}}
            <td class="preview">
                {{if Thumbnail_url}}
                    <img width="192" height="144" alt="${Name}" src="${Thumbnail_url}">
                {{/if}}
            </td>
            <td class="name">
                <a href="${url}"{{if thumbnail_url}} target="_blank"{{/if}}>${Name}</a>
            </td>
            <td class="size">${Length}</td>
            <td colspan="2"></td>
        {{/if}}
        <td class="delete">
            <button data-type="${delete_type}" data-url="${delete_url}">Delete</button>
        </td>
    </tr>
    </script>
    
<script type="text/javascript" src="//iamgds.com/cms/js/jquery.min.1.6.js"></script>
<script type="text/javascript" src="//iamgds.com/cms/js/jquery-ui.min.js"></script>
<script type="text/javascript" src="//iamgds.com/cms/js/jquery.tmpl.min.js"></script>
<script type="text/javascript" src="//iamgds.com/cms/js/jquery.iframe-transport.js"></script>
<script type="text/javascript" src="//iamgds.com/cms/js/jquery.fileupload.js"></script>
<script type="text/javascript" src="//iamgds.com/cms/js/jquery.fileupload-ui.js"></script>
<script type="text/javascript">
$(function() {
    var lstData;
    var datavalue = "m=gacs";
    $.ajax({
        url: "cms.ashx",
        type: "GET",
        data: datavalue
    }).done(function(data){
        lstCities = data.cities;
        $.each($.parseJSON(data.states), function(key,value){
            $('#ddState').append("<option value='"+value.state_id+"'>"+value.state_name+"</option>");
        });
    });

    var loadCities = function(stateId) {
        $('#ddCity').empty();
        $.each($.parseJSON(lstCities), function(key,value){
            if(value.state_id == stateId) {
                $('#ddCity').append("<option value='"+value.city_id+"'>"+value.city_name +"</option>");
            }
        });
    };

    $( "#ddState" ).change(function() {
        loadCities(this.value);
    });

    'use strict';
    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload({
        singleFileUploads: true,
        maxNumberOfFiles:1,
    });

    $('#fileupload').bind('fileuploaddone', function(e, data) {
        if (data.jqXHR.responseText || data.result) {
            var fu = $('#fileupload').data('fileupload');
            var JSONjQueryObject = (data.jqXHR.responseText) ? jQuery.parseJSON(data.jqXHR.responseText) : data.result;
            fu._adjustMaxNumberOfFiles(JSONjQueryObject.files.length);
            //                debugger;
            fu._renderDownload(JSONjQueryObject.files)
        .appendTo($('#fileupload .files'))
        .fadeIn(function() {
            // Fix for IE7 and lower:
            $(this).show();
        });
        }
    });

    $('#fileupload .files a:not([target^=_blank])').live('click', function(e) {
        e.preventDefault();
        $('<iframe style="display:none;"></iframe>')
            .prop('src', this.href)
            .appendTo('body');
    });

});

</script>

</body>
</html>
