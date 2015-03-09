// button click events
$("#srchdComp").keypress(function(event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        $('#btnSrhComp').click();
    }
});
$(document).on("click", "#btnSrhComp", function() {
    createCompanyDataTable($('#srchdComp').val());
});
$(document).on("click", "#save_features", function() {
    var fcb = "";
    $('input[name="cbFeature[]"]:checked').each(function(index, elem) {
        fcb += $(elem).val().replace("cb_", "") + ",";
        //checkboxValues.push($(elem).val());
    });
    var datavalue = "m=utf&features=" + fcb;
    $.ajax({
        type: "GET",
        url: "handler/trip.ashx",
        data: datavalue,
        success: function(data) {
            $('#feature_succes').show().fadeOut(5000);
        }
    });
});
$(document).on("click", "#btnAddBus", function() {
    var tbBusNoVal = $('#txtNewBusNo').val();
    if (tbBusNoVal === "") {
        alert("Bus No can not be empty.");
    } else {
        var datavalue = "m=mbt&bus_no=" + tbBusNoVal;
        $.ajax({
            type: "GET",
            url: "handler/trip.ashx",
            data: datavalue,
            success: function(data) {
                $('#added_bus').show().fadeOut(5000);
            }
        });
    }
});
$(document).on("click", "#btnMapBus", function() {
    var bus_no = $('#ddCompBuses').val();
    var datavalue = "m=mbt&bus_no=" + bus_no;
    $.ajax({
        type: "GET",
        url: "handler/trip.ashx",
        data: datavalue,
        success: function(data) {
            $('#added_bus').show().fadeOut(5000);
        }
    });
});

// ajax calls
var getAmenitiesList = function() {
    var datavalue = "m=gal"
    return $.ajax({
        url: "handler/trip.ashx",
        type: "GET",
        data: datavalue
    });
};
var getActiveBusOnTy = function(tripId) {
    var datavalue = "m=tab&trip_id=" + tripId;
    return $.ajax({
        url: "handler/trip.ashx",
        type: "GET",
        data: datavalue
    });
};
var fillBusesDropdown = function(trip) {
    var datavalue = "m=gbd&trip_id=" + trip.TripId; // +"&company_id=" + $("#hdnCompanyId").val();
    return $.ajax({
        url: "handler/trip.ashx",
        type: "GET",
        data: datavalue
    });
};

// datatables
var createCompanyDataTable = function(company_name) {
    $('#trips_buses').hide();
    $('#featues_upload').hide();
    $('#company_lable').hide();
    $('#trip_dom').hide();
    $('#trip_lable').hide();
    $('#company_dom').show();
    var ajaxSource = "handler/trip.ashx?m=gcl&company_name=" + company_name;
    $('#dtCompanies').dataTable({
        "sDom": "T<'row'<'col-md-6'l><'col-md-6'f>r>t<'row'<'col-md-6'i><'col-md-6'p>>",
        "bSortClasses": false,
        "sAjaxDataProp": "",
        "sAjaxSource": ajaxSource,
        "oLanguage": {
            "sLengthMenu": 'Display ' +
                '<select class="form-control">' +
                    '<option value="10">10</option>' +
                    '<option value="20">20</option>' +
                    '<option value="30">30</option>' +
                    '<option value="40">40</option>' +
                    '<option value="50">50</option>' +
                    '<option value="-1">All</option>' +
                '</select>' +
                'records'
        },
        //"aLengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
        //"oLanguage": { "sLengthMenu": "Display _MENU_ rows" },
        "iDisplayLength": 10,
        "bDestroy": true,
        "aaSorting": [[1, 'asc']],
        "aoColumns": [
                    { "mData": "CompanyID", "sTitle": "Id" },
                    { "mData": "CompanyName", "sTitle": "Company Name" },
                    { "mData": "City", "sTitle": "City", "sClass": "toCap" },
                    { "mData": "State", "sTitle": "State", "sClass": "toCap" }
                ],
        "fnRowCallback": function(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(nRow).on('click', function() {
                createTripDataTable(aData);
            });
        }
    });
    $('#dtCompanies_filter input').addClass('form-control');
};
var createTripDataTable = function(company_data) {
    //$('#hdnCompanyId').val(company_data.CompanyID);
    $('#trips_buses').hide();
    $('#featues_upload').hide();
    $('#trip_lable').hide();
    $('#company_dom').hide();
    $('#lblCompanyId').text(company_data.CompanyID);
    $('#lblCompanyName').text(company_data.CompanyName);
    $('#lblCompanyCity').text(company_data.City);
    $('#lblCompanyState').text(company_data.State);
    $('#company_lable').show();
    $('#trip_dom').show();
    var ajaxSource = "handler/trip.ashx?m=gct&company_id=" + company_data.CompanyID;
    $('#dtTrips').dataTable({
        "sDom": "<'row'<'col-md-6'l><'col-md-6'f>r>t<'row'<'col-md-6'i><'col-md-6'p>>",
        "sAjaxDataProp": "",
        "sAjaxSource": ajaxSource,
        "oLanguage": {
            "sLengthMenu": 'Display ' +
                '<select class="form-control">' +
                    '<option value="10">10</option>' +
                    '<option value="20">20</option>' +
                    '<option value="30">30</option>' +
                    '<option value="40">40</option>' +
                    '<option value="50">50</option>' +
                    '<option value="-1">All</option>' +
                '</select>' +
                'records'
        },
        "iDisplayLength": 10,
        "bDestroy": true,
        "aoColumns": [
                    //{ "mData": "GDSTripId", "sTitle": "Trip Id", "sWidth": "04%" },
                    { "mData": "TripId", "sTitle": "Trip Id", "sWidth": "04%" },
                    { "mData": "RouteInfo", "sTitle": "Route Info", "sWidth": "70%" },
                    { "mData": "DepartureTime", "sTitle": "Departure Time", "sWidth": "10%", "bSortable": false },
                    { "mData": "Amenities", "sTitle": "Amenities", "sWidth": "12%", "bSearchable": false, "bSortable": false, "mRender": function(data, type, full) { return showAmenitiesIcon(data); } },
                    { "mData": "hasPic", "sTitle": "Pics", "sWidth": "02%", "bSearchable": false, "bSortable": false, "mRender": function(data, type, full) { var tag = isPics(data); return tag; } },
                    { "mData": "busCnt", "sTitle": "Buses", "sWidth": "02%" },
                    { "mData": "picCnt", "sTitle": "Pics", "sWidth": "02%" }
                ],
        "fnRowCallback": function(nRow, aData, iDataIndex) {
            $(nRow).on('click', function() {
                showFeaturesForm(aData).done(function(data) {
                    setCheckedFeatures(data);
                    $("#featues_upload").show();
                });
                fillBusesDropdown(aData).done(function(tripdata) {
                    $('#ddCompBuses').empty();
                    var bn = $.parseJSON(tripdata);
                    $.each(bn.CompanyBuses, function(key, value) {
                        var val = value.bus_no.replace(/ /g, "_");
                        $('#ddCompBuses').append("<option value='" + val + "'>" + value.bus_no + "</option>");
                    });
                    var elemTabBus = $('#tabBuses');
                    elemTabBus.empty().append("<thead><tr><th width='50%'>Bus No</th><th width='20%'>Pics Count</th><th width='30%'>&nbsp;</th></tr></thead><tbody>");
                    $.each(bn.TripBuses, function(key, value) {
                        elemTabBus.append("<tr><td>" + value.bus_no + "</td><td>" + value.picCnt + "</td><td><span class='input-group-btn'><a href='buses.aspx?busno=" + value.bus_no.replace(/ /g, '_') + "&compid=" + bn.CompanyId + "' target='_blank' class='btn btn-sm btn-default' role='button'><span class='glyphicon glyphicon-eye-open'>&nbsp;</span>Upload Images!</a></span></td></tr>");
                    });
                    elemTabBus.append('</tbody>');
                    $("#trips_buses").show();
                });
            });
        }
    });
    $('#dtTrips_filter input').addClass('form-control');
};

// functions
var createAmenitiesList = function() {
    getAmenitiesList().done(function(result) {
        try {
            var alHtml = "";
            $(result).each(function(key, value) {
                alHtml += "<div class='col-md-3'><div class='checkbox'><label><input checked='checked' type='checkbox' name='cbFeature[]' value='cb_" + value.code + "' />" + value.name + "</label></div></div>";
            });
            $("#amnlst").replaceWith(alHtml);
        } catch (e) {
            alert(e);
        }
    });
};
var showFeaturesForm = function(trip_data) {
    $('#trip_dom').hide();
    $('#lblTripId').text(trip_data.TripId);
    $('#lblRouteInfo').text(trip_data.RouteInfo);
    if (trip_data.DepartureTime !== null)
        $('#lblDepartureTime').text(trip_data.DepartureTime); //.Hours + ':' + trip_data.DepartureTime.Minutes);

    getActiveBusOnTy(trip_data.TripId).done(function(result) {
        var r = $.parseJSON(result)[0];
        try {
            $('#tyActiveBus').attr("href", "buses.aspx?busno=" + r.bus_no.replace(/ /g, "_") + "&compid=" + r.company_id);
            $('#tyActiveBus').show();
        } catch (e) {
            $('#tyActiveBus').hide();
        }
    });

    $('#trip_lable').show();
    $('#feature_succes').hide();
    var datavalue = "m=gtf&trip_id=" + trip_data.TripId; // +"&company_id=" + $("#hdnCompanyId").val();
    return $.ajax({
        url: "handler/trip.ashx",
        type: "GET",
        data: datavalue
    });
};
var setCheckedFeatures = function(amneties) {
    var amenitylist = $.parseJSON(amneties)[0].amenities
    $("input[name='cbFeature[]'").removeAttr('checked');
    $("input[name='cbFeature[]'").each(function() {
        if (amenitylist.toLowerCase().indexOf($(this).val().replace("cb_", "") + ",") >= 0) {
            $(this).prop('checked', true); //.attr("checked", "checked");
        }
    });
};
var showDTCompany = function() {
    $('#trips_buses').hide();
    $('#featues_upload').hide();
    $('#trip_lable').hide();
    $('#trip_dom').hide();
    $('#company_lable').hide();
    $('#company_dom').show();
};
var showDTTrip = function() {
    $('#trips_buses').hide();
    $('#featues_upload').hide();
    $('#trip_lable').hide();
    $('#trip_dom').show();
};
var isPics = function(data) {
    if (data) {
        return '<span class="glyphicon glyphicon-ok text-success"></span>';
    } else {
        return '<span class="glyphicon glyphicon-ban-circle text-danger"></span>';
    }
};
var showAmenitiesIcon = function(strAmenites) {
    var amenties = "";
    if (strAmenites != "") {
        if (/tv,/i.test(strAmenites)) { amenties += "<span title='TV' class='icon tv'>&nbsp;</span>"; }
        if (/wf,/i.test(strAmenites)) { amenties += "<span title='WiFi' class='icon wifi'>&nbsp;</span>"; }
        if (/sk,/i.test(strAmenites)) { amenties += "<span title='Snacks' class='icon snacks'>&nbsp;</span>"; }
        if (/tl,/i.test(strAmenites)) { amenties += "<span title='Toilet' class='icon toilet'>&nbsp;</span>"; }
        if (/bk,/i.test(strAmenites)) { amenties += "<span title='Blanket' class='icon blanket'>&nbsp;</span>"; }
        if (/wb,/i.test(strAmenites)) { amenties += "<span title='Water Bottle' class='icon bottle'>&nbsp;</span>"; }
        if (/cp,/i.test(strAmenites)) { amenties += "<span title='Charging Point' class='icon charging'>&nbsp;</span>"; }
        if (/pt,/i.test(strAmenites)) { amenties += "<span title='Personalize TV' class='icon movie'>&nbsp;</span>"; }
        if (/gp,/i.test(strAmenites)) { amenties += "<span title='Personalize GPS' class='icon gps'>&nbsp;</span>"; }
        if (/np,/i.test(strAmenites)) { amenties += "<span title='Personalize Newspaper' class='icon newspaper'>&nbsp;</span>"; }
    } else {
        amenties = "<span title='No Amenities' class='glyphicon glyphicon-ban-circle text-danger'></span>";
    }
    return amenties;
};
var showBusesInfo = function(busNo, CompId) {
    var url = "buses.aspx?busno=" + busNo + "&compid=" + CompId;
    document.location = url;
    return;
};

// initilizer
$(function() {
    createAmenitiesList();
    createCompanyDataTable("");
});
