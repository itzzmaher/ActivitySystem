
function Valdation() {
        $("#form input, #form select,#form textarea ").each(function () {
            if ($(this).val() == "" || $(this).val() == null) {
                if (!$(this).hasClass('note-input') && !$(this).hasClass('note-codable')) {
                    $(this).addClass("is-invalid");
                }
                else {
                    $(this).removeClass("is-invalid");
                }
            }
            //else if ($(this).attr('name') == 'MobileNumber') {
            //    if ($(this).val().length == 10)
            //        $(this).removeClass("is-invalid");
            //    else
            //        $(this).addClass("is-invalid");
            //}
            else {
                $(this).removeClass("is-invalid");
            }

        });
    if ($("#form .is-invalid").length == 0) {
        return true;
    }
    else {
        return false;
    }
}
