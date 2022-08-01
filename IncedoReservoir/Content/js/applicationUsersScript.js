$(document).ready(function () {
    $('#tblCustomerInfo').DataTable(
        {
            searching: false,
            "bFilter": false,
            "bLengthChange": true,            
            "order": [],
            "columnDefs": [{ orderable: false, targets: -1 }]
        });
});

jQuery(document).ready(function ($) {
    $(document).keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            $('#btnSearch').click();
        }
    });
});


jQuery(document).ready(function ($) {
    $('#txtUserLoginNameSearch').keypress(function (e) {
        var regex = new RegExp("^[0-9a-zA-Z]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }
        e.preventDefault();
        return false;
    });
});

jQuery(document).ready(function ($) {
    $('#txtUserDisplayNameSearch').keypress(function (e) {
        var regex = new RegExp("^[0-9a-zA-Z_\.]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }
        e.preventDefault();
        return false;
    });
});

jQuery(document).ready(function ($) {
    $('#inputUserLoginName').keypress(function (e) {
        var regex = new RegExp("^[0-9a-zA-Z]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }
        e.preventDefault();
        return false;
    });
});

jQuery(document).ready(function ($) {
    $('#inputUserDisplayName').keypress(function (e) {
        var regex = new RegExp("^[0-9a-zA-Z_\.]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }
        e.preventDefault();
        return false;
    });
});

jQuery(document).ready(function ($) {
    $('#inputUserLoginNameEdit').keypress(function (e) {
        var regex = new RegExp("^[0-9a-zA-Z]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }
        e.preventDefault();
        return false;
    });
});

jQuery(document).ready(function ($) {
    $('#inputUserDisplayNameEdit').keypress(function (e) {
        var regex = new RegExp("^[0-9a-zA-Z_\.]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }
        e.preventDefault();
        return false;
    });
});











$(document).ready(function () {
    $('#inputPassword').keyup(function () {
        $('#strength_messageoneadd').html(checkStrength($('#inputPassword').val(), '#strength_messageoneadd'))
    })

    $('#inputPasswordEdit').keyup(function () {
        $('#strength_messageoneedit').html(checkStrength($('#inputPasswordEdit').val(), '#strength_messageoneedit'))
    })

    function checkStrength(password, controlname) {
        var strength = 0
        if (password.length < 6) {
            $('' + controlname + '').removeClass()
            $('' + controlname + '').addClass('text-danger pull-right')
            return 'To Short Password Detected !!'
        }
        var pattern = new RegExp("[-0-9a-zA-Z.+]+@@[-0-9a-zA-Z.+]+\.[a-zA-Z]{2,4}");
        if (password.length > 7) strength += 1
        if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) strength += 1
        if (password.match(/([a-zA-Z])/) && password.match(/([0-9])/)) strength += 1
        if (password.match(/([!,%,&,@@,#,$,^,*,?,_,~])/)) strength += 1
        if (password.match(pattern)) strength += 1
        if (strength < 2) {
            $('' + controlname + '').removeClass()
            $('' + controlname + '').addClass('text-danger pull-right')
            return 'Week Password Detected !!'
        }
        else if (strength == 2) {
            $('' + controlname + '').removeClass()
            $('' + controlname + '').addClass('text-orange pull-right')
            return 'Good Combination Detected In Password, Try More !!'
        }
        else {
            $('' + controlname + '').removeClass()
            $('' + controlname + '').addClass('text-green pull-right')
            return 'Strong & Perfect Combination In Password Detected !!'
        }
    }
});