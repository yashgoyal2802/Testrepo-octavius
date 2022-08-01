
    data; // ReferenceError: data is not defined
var data;
data; // No more errors

jQuery(document).ready(function ($) {
    $('#txtSearchGroupName').keypress(function (e) {
        var regex = new RegExp("^[0-9a-zA-Z-]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }
        e.preventDefault();
        return false;
    });
});

$(document).ready(function () {
    $('#txtSearchGroupName').keypress(function (e) {
        if (e.keyCode == 13)
            $('#btnSearch').click();
    });
       
    $('#tblCustomerInfo').DataTable(
        {
            searching: false,
            "bFilter": false,
            "bLengthChange": false
        });
      
});


jQuery(document).ready(function ($) {
    $('#txtPrivilegeGroupName').keypress(function (e) {
        var regex = new RegExp("^[0-9a-zA-Z-]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }
        e.preventDefault();
        return false;
    });
});

$(document).ready(function () {
    $('#txtPrivilegeGroupName').keypress(function (e) {
        if (e.keyCode == 13)
            $('#btnadd').click();
    });
});

    

$('#btnSearch').on('click', function (evt) {
    evt.preventDefault();
    evt.stopPropagation();
    var strSearchGroupName = $("#txtSearchGroupName").val();
    if (strSearchGroupName == "") {
        $.ajax({
            url: '@Url.Action("PrivilegeGroupList", "PrivilegeGroup")',
            data: { sSearchGroupName: "", iAction: 2 },
            cache: false,
            type: "GET",
            dataType: "html",
            success: function (data) {
                $('#PrivilegeGroupDetails').html(data);
                $('#searchErrorMsg').show();
                $('#searchErrorMsg').text('Please enter any of search criteria to get Privilege Group information !!');
                $("#searchErrorMsg").css("alert alert-danger");
            }
        });
        return true;
    }
    else {
        $.ajax({
            url: '@Url.Action("PrivilegeGroupList", "PrivilegeGroup")',
            data: { sSearchGroupName: strSearchGroupName, iAction: 2 },
            cache: false,
            type: "GET",
            dataType: "html",
            success: function (data) {
                $('#PrivilegeGroupDetails').html(data);
                $('#searchErrorMsg').hide();
                $('#searchErrorMsg').text('');
                $("#searchErrorMsg").css("alert alert-success");
            }
        });
    }
});

function checkPrivilegeNameAdd() {
    var txtPrivilegeGroupName = $("#txtPrivilegeGroupName").val();

    if (txtPrivilegeGroupName == "") {
        return true;
    }       
    $.ajax({
        url: '@Url.Action("ValidatePrivilegeGroupName", "PrivilegeGroup")',
        data: { strPrivilegeGroupName: txtPrivilegeGroupName },
        cache: false,
        type: "GET",
        dataType: "html",
        success: function (data) {
            if (data != 0 && data > 0) {
                $('#addErrorMsg').show();
                $("#btnadd").attr("disabled", true);
                $('#addErrorMsg').text('Privilege Group Name already exist ! Please try with different Privilege Group Name !');
                return true;
            }
            else {
                $('#addErrorMsg').hide();
                $("#btnadd").attr("disabled", false);
                $('#addErrorMsg').text('');
                return true;
            }
        }
    });
        
}

function AddPrivilegeGroup() {
    var txtPrivilegeGroupName = $('#txtPrivilegeGroupName').val();
    var txtAddFL = $('#txtAddFL').val();
    var checkedVals = $('.AddCheck:checkbox:checked').map(function () {
        return this.value;
    }).get();

       
    var chkbox = checkedVals.join(",");
    if (chkbox == null || chkbox == "") {
        $('#addErrorMsg').show();
        $('#addErrorMsg').text('Please check / select atleast one Privilege Level to create new  Privilege Group !!');
        $("#addErrorMsg").css("alert alert-danger");
        return false;
    }
    else {
        $('#addErrorMsg').hide();
        $('#addErrorMsg').text('');
        $("#addErrorMsg").css("alert alert-sucess");
    }

    $.ajax({
        type: "POST",
        url: '@Url.Action("AddPrivilegeGroup", "PrivilegeGroup")',
        data: '{"SGN":"' + txtPrivilegeGroupName + '","FL":"' + txtAddFL + '","Checkval":"' + chkbox + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {               
            document.getElementById('lightAdd').style.display = 'none';
            document.getElementById('fade').style.display = 'none'
            $('#txtPrivilegeGroupName').val('');
            $('#addErrorMsg').hide();               
            $('#addErrorMsg').text('');

            $('#searchErrorMsg').show();             
            $('#searchErrorMsg').text('Greetings !! Privilege Group' + txtPrivilegeGroupName + ' added successfully !!');
            $("#searchErrorMsg").css("alert alert-success");
            location.reload();
        },
        error: function (response) {
            $('#addErrorMsg').show();
            //$("#btnadd").attr("disabled", true);
            $('#addErrorMsg').text('There is some issue with data save !');
            $('#searchErrorMsg').show();
            $('#searchErrorMsg').text('There is some issue while adding New Privilege Group, Please connect with SUpport Team !!');
            $("#searchErrorMsg").css("alert alert-danger");
        }
    });
}


function editPrivilegeGroup(e) {
    document.getElementById('lightEdit').style.display = 'block';
    document.getElementById('fade').style.display = 'block';
    var gid = e.dataset.id;
    $.ajax({
        type: "POST",
        url: '@Url.Action("EditPrivilegeGroup", "PrivilegeGroup")',
        data: '{"id":"' + gid + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {           
            var d = response.CheckValue;
            $(".EditCheck").prop('checked', false);
            if (d != null || d != undefined) {
                d = d.substring(0, d.length - 1)
                var f = d.split(",");
                jQuery.each(f, function (i, val) {                       
                    $("#" + val).prop('checked', true);
                });                    
            }
            $('#hdneditServiceGroupName').val(response.GroupName);
            $('#txteditServiceGroupName').val(response.GroupName);
            $('#txteditFL').val(response.FunctionalLevel);
            $('#btnupdate').attr('data-id', response.GroupID);
            $('#btnupdate').attr('data-privilege', d);

        },
        error: function (response) {
            alert(response);
        }
    });
}

function checkPrivilegeNameEdit() {
    var txtPrivilegeGroupName = $("#txteditServiceGroupName").val();
    var hdneditServiceGroupName = $("#hdneditServiceGroupName").val();

    if (txtPrivilegeGroupName == "") {
        $('#editErrorMsg').show();
        $('#editErrorMsg').text('Privilege Group Name cant be empty or null. !!');
        return false;
    }
    else {
        $('#editErrorMsg').hide();
        $('#editErrorMsg').text('');
    }
    if (hdneditServiceGroupName != txtPrivilegeGroupName) {
        $.ajax({
            url: '@Url.Action("ValidatePrivilegeGroupName", "PrivilegeGroup")',
            data: { strPrivilegeGroupName: txtPrivilegeGroupName },
            cache: false,
            type: "GET",
            dataType: "html",
            success: function (data) {
                if (data != 0 && data > 0) {
                    $('#editErrorMsg').show();
                    //$("#btnadd").attr("disabled", true);
                    $('#editErrorMsg').text('Privilege Group Name cant be empty, Please enter Privilege Group Name !!');
                    return true;
                }
                else {
                    $('#editErrorMsg').hide();
                    //$("#btnadd").attr("disabled", false);
                    $('#editErrorMsg').text('');
                    return true;
                }
            }
        });
    }
}


jQuery(document).ready(function ($) {
    $('#txteditServiceGroupName').keypress(function (e) {
        var regex = new RegExp("^[0-9a-zA-Z-]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }
        e.preventDefault();
        return false;
    });
});

$(document).ready(function () {
    $('#txteditServiceGroupName').keypress(function (e) {
        if (e.keyCode == 13)
            $('#btnupdate').click();
    });
});


function UpdatePrivilegeGroup() {
       
    var txtPrivilegeGroupName = $('#txteditServiceGroupName').val();
    var txtAddFL = 'SO-USER';
    var txtEditGroupID = $('#btnupdate').attr('data-id');
    var Editprivilege = $('#btnupdate').attr('data-privilege');
    var checkedVals = $('.EditCheck:checkbox:checked').map(function () {
        return this.value;
    }).get();

    if (txtPrivilegeGroupName == "")
    {
        $('#editErrorMsg').show();
        //$("#btnadd").attr("disabled", true);
        $('#editErrorMsg').text('Privilege Group Name cant be empty, Please enter Privilege Group Name !!');
        return false;
    }

    var chkbox = checkedVals.join(",");
    $.ajax({
        type: "POST",
        url: '@Url.Action("UpdatePrivilegeGroup", "PrivilegeGroup")',
        data: '{"SGN":"' + txtPrivilegeGroupName + '","FL":"' + txtAddFL + '","Checkval":"' + chkbox + '","GroupID":"' + txtEditGroupID + '","PrivilegesID":"' + Editprivilege + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            document.getElementById('lightEdit').style.display = 'none';
            document.getElementById('fade').style.display = 'none'
        },
        error: function (response) {
            //alert(response);
        }
    });
}
