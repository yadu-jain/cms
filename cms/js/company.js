var hndlrComp = "handler/company.ashx";
var saveSEODetail = function(){
    debugger;
    var csdId = $('#hdnCSDId').val();
    var compId = $('#hdnCompId').val();
    var compSEOname = $('#txtCSN').val();
    var seourl = $('#txtSeoUrl').val();
    var statWrite = $('.summernote').code();
    var datavalue = { m: "scsd", Id: csdId, CompanyId: compId, SEOName: compSEOname, SeoUrl: seourl, WriteUp: statWrite };
    return $.ajax({
        url: hndlrComp,
        type: "GET",
        data: datavalue
    });
};
$(document).on("click", "#btnSaveSEODtl", function(){
    debugger;
    saveSEODetail().done(function(res){
        alert("theri");
    });
});
var showSEODetail = function(company_id) {
    debugger;
    $('#hdnCompId').val(company_id);
    var datavalue = "m=sd&company_id="+company_id;
    return $.ajax({
        url: hndlrComp,
        type: "GET",
        data: datavalue
    });
};
$(document).on("click", "#btnSeoDetail", function() {
    showSEODetail($("#ddCompanies").val()).done(function(res) {
        // hide other dom and button
        $('#divCompanyForm').hide();
        $('#viewCompanyInfo').hide();
        $('#divBranches').hide();
        // show seo detail dom
        $('#divCompSEODtls').show();
        // empty control before filling any data
        $('#txtCSN').val('');
        $('#txtSeoUrl').val('');
        $('.summernote').summernote({ height: 300 });
        $('.summernote').code('');
        if(!$.isEmptyObject(res)) {
            var csd = res[0];
            // fill data in controls
            $('#hdnCSDId').val(csd.Id);
            $('#lblGDSCompanyName').text(csd.CompanyName);
            $('#txtCSN').val(csd.SEOCompanyName);
            $('#txtSeoUrl').val(csd.SeoUrl);
            $('.summernote').summernote({ height: 300 });
            $('.summernote').code(csd.StaticWriteup);
        } else {
            var complab = $("#ddCompanies").find('option:selected').text();
            $('#lblGDSCompanyName').text(complab);
            $('#hdnCSDId').val(0);
        }
    });
    
    $("#divCompanyForm").hide();
    $("#viewCompanyInfo").hide();
    $("#divBranches").hide();
});
var loadStateCity = function() {
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
};

var showBranches = function(company_id) {
    //if ($('#dtBranches').dataTable() != null) { $('#dtBranches').dataTable().fnDestroy(); }
    //oTable = 
    $('#dtBranches').dataTable({
        "sDom": "<'row'<'col-xs-9'T><'col-xs-3'f>r>t<'row'<'col-xs-6'i><'col-xs-6'p>>",
        "bSortClasses": false,
        "sAjaxDataProp": "",
        "sAjaxSource": "cms.ashx?m=gcb&company_id=" + company_id,
        //"sPaginationType": "full_numbers",
        //"iDisplayLength": 20,
        //"bServerSide": true,
        "bDestroy": true,
        "oLanguage": {
            "sEmptyTable" : "No braches addes yet. Please add new branch..."
        },
        "aoColumns": [
                    { "mData": "branch_id", "sTitle": "Branch Id" },
                    { "mData": "name", "sTitle": "Branch Name" },
                    { "mData": "address", "sTitle": "Address" },
                    { "mData": "city_id", "bVisible": false },
                    { "mData": "city_name", "sTitle": "City", "sClass": "toCap"  },
                    { "mData": "state_id", "bVisible": false },
                    { "mData": "state", "sTitle": "State", "sClass": "toCap" },
                    { "mData": "pin", "sTitle": "PinNO" },
                    { "mData": "contact", "sTitle": "Contact" }
                    //{ "mData": "email", "sTitle": "Email" },
                    //{ "mData": "manager", "sTitle": "Manager" },
                    //{ "mData": "manager_contact", "sTitle": "Manager Contact" }
                ],
        "fnRowCallback": function(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(nRow).on('click', function() {
                $("#hdnBId").val(aData["branch_id"]);
                loadStateCity();
                //debugger;
                console.log(aData);
                debugger;
                $("#ddState").val(aData["state_id"]);
                $("#txtBName").val(aData["name"]);
                $("#taBranch").val(aData["address"]);
                $("#txtBPin").val(aData["pin"]);
                $("#txtBCont").val(aData["contact"]);
                //$("#txtBMail").val(aData["email"]);
                //$("#txtBMang").val(aData["manager"]);
                //$("#txtBMCont").val(aData["manager_contact"]);
                $('#branchModel').modal('show');
                $("#ddCity").val(aData["city_id"]).text(aData["city_name"]).attr("selected", "selected");
                //console.log(aData);
            });
        }
    });
    $('#dtBranches_filter input').addClass('form-control');
    $('#btnAddNewBranch').click( function () {
        debugger;
        $("#hdnBId").val(0);
        loadStateCity();
        
    } );
};
var SaveBranch = function() {
        var bId, bName, bAdd, bCt, bSt, bPin, bCont;// bEmail, bMan, bMCont;
        bId = $("#hdnBId").val();
        bName = $("#txtBName").val();
        bAdd = $("#taBranch").val();
        bCt = $("#ddCity").val();
        bSt = $("#ddState").val();
        bPin = $("#txtBPin").val();
        bCont = $("#txtBCont").val();
        //bEmail = $("#txtBMail").val();
        //bMan = $("#txtBMang").val();
        //bMCont = $("#txtBMCont").val();
        
        $.ajax({
            type: "GET",
            url: 'cms.ashx',
            dataType: "json",
            data: { m: "scb", Id: bId, Name: bName, Add: bAdd, Ct: bCt, St: bSt, Pin: bPin, Cont: bCont }, //Email: bEmail, Man: bMan, MCont: bMCont },
            success: function (result) {
                $('#branchModel').modal('hide');
                //console.log(result);
                //debugger;
                showBranches(result.company_id);
            },
            error: function (msg) {
                //ShowProgressBar(false);
            }
        });
};
$(document).on("click", "#btnBranches", function() {
    //$("#btnBranches").hide();
    //$("#btnSaveBranches").show();
    showBranches($("#ddCompanies").val());
    
    $("#divCompanyForm").hide();
    $("#viewCompanyInfo").hide();
    $("#divBranches").show();
    /*showBranches($("#ddCompanies").val()).done(function(data){
        //fillCityStateTemp();
        if($.parseJSON(data).res == 0) {
        } else {
        }
        $("#divCompanyForm").hide();
        $("#viewCompanyInfo").hide();
        $("#divBranches").show();
    });*/
});
var loadCompanyInfo = function(company_id) {
    var datavalue = "m=gci&company_id="+company_id;
    return $.ajax({
        type: "GET",
        url: "cms.ashx",
        data: datavalue,
    });
};
$(document).on("click", "#btnCompany", function() {
    //$("#btnBranches").show();
    //$("#btnSaveBranches").hide();
    var company = $('#ddCompanies').val();
    $("#divBranches").hide();
    //$("#new_company_info").contents().find("#hdnCompanyId").val(company);
    loadCompanyInfo(company).done(function(data) {
        if($.parseJSON(data).res == 0) {
            $("#viewCompanyInfo").hide();
            $("#divCompanyForm").show();
        } else {
            var rs = $.parseJSON(data)[0];
            //console.log(rs);
            //debugger;
            for(var key in rs) { 
                $("#"+key).text(rs[key]); 
                /*if(key == "comapany_email")
                    $("#"+key).attr("href", "mailto:"+rs[key]);
                else*/ 
                if(key == "logo_link")
                    $("#"+key).attr("src", rs[key]);
            }
            $("#divCompanyForm").hide();
            $("#viewCompanyInfo").show();
        }
        
    });
});
var loadProvidersCompany = function(provider_id) {
    datavalue = "m=gpc&provider_id=" + provider_id;
    return $.ajax({
        type: "GET",
        url: "cms.ashx",
        data: datavalue,
    });
};
$(document).on("click", "#btnProvider", function() {
    var provider = $('#ddProviders').val();
    loadProvidersCompany(provider).done(function(data) {
        $("#viewCompanyInfo").hide();
        $("#divCompanyForm").hide();
        
        $("#ddCompanies").empty();
        $("#ddCompanies").append(data);
    });
});
var loadProviders = function() {
    datavalue = "m=gpd";
    return $.ajax({
        url: "cms.ashx",
        type: "GET",
        data: datavalue
    });
};
$(function() {
    loadProviders().done(function(data) {
        $("#viewCompanyInfo").hide();
        $("#divCompanyForm").hide();
        
        $("#ddProviders").empty();
        $("#ddProviders").append(data);
    });
});
