// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.




ShowInPopUP = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#form-modal .modal-body").html(res);
            $("#form-modal .modal-title").html(title);
            $("#form-modal").modal('show');
        }



    });

    jQueryAjaxPost = form => {
        $.ajax({
            type: "POST",
            url: url,
            data: new formData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                    /*$("#Index").html(res.html);*/
                    $("#form-modal .modal-body").html('');
                    $("#form-modal .modal-title").html('');
                    $("#form-modal").modal('hide');
            },
            error: function (err) {
                console.log()
            }

        })

    }
    // to prevent default form submit event
  //  return false;

}


jqueryAjaxDelete = form => {
    if (confirm('Are Sure To Delete This Record')) {
        try {
            $.ajax({
                type: "POST",
                url: url,
                data: new formData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $("#Index").html(res.html);
                },
                error: function (err) {
                    console.log(err);
                }
            })

        } catch (e) {
            console.log(e);
        }

    }
}

ShowModelPopUp = (url, title) => {
    $.ajax({
        method:"GET",
        url: url,
        success: function (res) {
            $("#form-modal .modal-body").html(res);
            $("#form-modal .modal-title").html(title);
            $("#form-modal").modal('show');
        }


    });
}

// Example starter JavaScript for disabling form submissions if there are invalid fields
//(function () {
//    'use strict'

//    // Fetch all the forms we want to apply custom Bootstrap validation styles to
//    var forms = document.querySelectorAll('.needs-validation')

//    // Loop over them and prevent submission
//    Array.prototype.slice.call(forms)
//        .forEach(function (form) {
//            form.addEventListener('submit', function (event) {
//                if (!form.checkValidity()) {
//                    event.preventDefault()
//                    event.stopPropagation()
//                }

//                form.classList.add('was-validated')
//            }, false)
//        })
//})()







