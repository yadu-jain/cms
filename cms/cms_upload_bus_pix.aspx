<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cms_upload_bus_pix.aspx.cs" Inherits="cms.cms_upload_bus_pix" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Upload Bus Images</title>
    
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
    <div class="panel panel-default">
        <div class="panel-heading">Bus Nos:</div>
        <div class="panel-body">
            <div id="fileupload"  style="min-height:580px;max-height:580px;">
                <form action="cms.ashx" method="POST" enctype="multipart/form-data">
                    <input type="hidden" id="hdnBusNo" runat="server" />
                    <input type="hidden" id="hdnCompanyId" runat="server" />
                    <div class="row" style="margin-top:4px;overflow:auto;">
                        <div class="col-md-12">
                            <div class="fileupload-buttonbar">
                                <label class="fileinput-button">
                                    <span>Add files...</span>
                                    <input id="file" type="file" name="files[]" multiple />
                                </label>
                                <button type="submit" class="start">Start upload</button>
                                <button type="reset" class="cancel">Cancel upload</button>
                                <button type="button" class="delete">Delete files</button>
                            </div>
                        </div>
                    </div>
                </form>
                <div class="fileupload-content">
                    <table id="busThumbView" class="files"></table>
                    <div class="fileupload-progressbar"></div>
                </div>
                <div id="busno_fail"  style="display:none;">
                    <div class="alert alert-danger"><strong>Select atleast 1 Bus!</strong></div>
                </div>
            </div>
        </div>
    </div>
</div>
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
        'use strict';
        $('#fileupload').fileupload({
        });
        $('#fileupload').bind('fileuploaddone', function(e, data) {
            if (data.jqXHR.responseText || data.result) {
                var fu = $('#fileupload').data('fileupload');
                var JSONjQueryObject = (data.jqXHR.responseText) ? jQuery.parseJSON(data.jqXHR.responseText) : data.result;
                fu._adjustMaxNumberOfFiles(JSONjQueryObject.files.length);
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
