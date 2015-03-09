$('#imgUlMdl').on('hide.bs.modal', function(e) {
    $("#cms_ul_img").contents().find("#busThumbView tr").remove();
    location.reload();
});
var activateBus = function(compid, busno) {
    var datavalue = "m=abn&bus_no=" + busno
    return $.ajax({
        type: "GET",
        url: "cms.ashx",
        data: datavalue
    });
};
$(document).on("click", "#btnActiveBus", function() {
    var conf = confirm("Are you sure?");
    if (!conf) { return; }
    var compid = getParameterByName("compid");
    var busno = getParameterByName("busno");

    activateBus(compid, busno).done(function(res) {
        alert("Activated successfully....");
    });
});
$(document).on("click", ".img-thumb", function() {
    $(this).toggleClass("bg-primary");
});
var saveBusPicTag = function(picData) {
    var datavalue = "m=sbt&save_data=" + JSON.stringify(picData);
    return $.ajax({
        type: "GET",
        url: "cms.ashx",
        data: datavalue
    });
};
$(document).on("click", "#btnGalSave", function() {
    var conf = confirm("Are you sure?");
    if (!conf) { return; }
    var cbSave = $("#gallery-thumb select");
    var toSave = {};
    $.each(cbSave, function(index, element) {
        var elem = $(element);
        if (elem.val() != null) {
            var i = elem.attr("id").replace("ddtag-", "");
            toSave[i] = elem.val();
        }
    });
    saveBusPicTag(toSave).done(function(res) {
        location.reload();
    });
});
var deleteBusPics = function(picIds) {
    var datavalue = "m=dbp&pic_Ids=" + picIds;
    return $.ajax({
        type: "GET",
        url: "cms.ashx",
        data: datavalue
    });
};
$(document).on("click", "#btnDelete", function() {
    var conf = confirm("Are you sure?");
    if (!conf) { return; }
    var cbDelete = $("#gallery-thumb .bg-primary");
    var toDel = new Array();
    $.each(cbDelete, function(index, element) {
        toDel[index] = element.id.replace("thumb-", "");
    });
    deleteBusPics(toDel).done(function(res) {
        location.reload();
    });
});
var createThumbTemplate = function(link, key) {
    var htmlStr = '<div class="col-md-3 img-thumb" id="thumb-' + key + '">';
    htmlStr += '<div class="input-group">';
    htmlStr += '<div class="thumb-align"><img src="' + link + '" alt="..." class="img-responsive img-thumbnail"></div>';
    htmlStr += '<div class="input-group center-block">';
    //htmlStr += '<input class="form-control" type="checkbox" name="cbImage">';
    //htmlStr += '<div class="checkbox"><label><input name="cbImage[]" value="'+key+'" type="checkbox"> Select Image</label></div>';
    htmlStr += '<select id="ddtag-' + key + '" class="form-control">';
    htmlStr += '<option value=""></option>';
    htmlStr += '<option value="front">Front</option>';
    htmlStr += '<option value="inside">Inside</option>';
    htmlStr += '<option value="side">Side</option>';
    htmlStr += '<option value="back">Back</option>';
    htmlStr += '</select>';
    htmlStr += '</div>';
    htmlStr += '</div>';
    htmlStr += '</div>';
    return htmlStr;
};
var getParameterByName = function(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
};
var getBusImages = function(bus_no, comp_id) {
    var datavalue = "m=gpt&bus_no=" + bus_no + "&company_id=" + comp_id;
    return $.ajax({
        type: "GET",
        url: "cms.ashx",
        data: datavalue
    });
};
$(function() {
    var busno = getParameterByName("busno");
    $("#lblBusNo").text(busno.replace(/_/g, " "));
    var compid = getParameterByName("compid");
    $("#cms_ul_img").contents().find("#hdnBusNo").val(busno);
    $("#cms_ul_img").contents().find("#hdnCompanyId").val(compid);
    getBusImages(busno, compid).done(function(data) {
        $("#gallery-thumb").empty();
        $.each($.parseJSON(data), function(key, value) {
            var ddelem = createThumbTemplate(value.link, value.keyVal);
            $("#gallery-thumb").append(ddelem);
            $("#ddtag-" + value.keyVal).val(value.tag);
        });
    });
});