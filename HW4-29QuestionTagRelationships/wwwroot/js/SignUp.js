$(() => {

    $("#btn-signup").prop('disabled', true);     //starts off, can't use the submit button

    let emailValid = false;                      //starts off that no email address entered will be not valid

    $("#email").on('blur', function () {         
        const email = $("#email").val();
        $.get(`/home/emailAvailable?email=${email}`, function (obj) {
            if (obj.isAvailable) {
                $("#email-unavailable").hide();
                emailValid = true;
            } else {
                $("#email-unavailable").show();
                emailValid = false;
            }
        })
    });     //when leaves email box, checks if the email address is available and sets emailValid    

    $("#password").on('keyup', () => {
        $("#btn-signup").prop('disabled', !isValidated());
    });       //when type in password box, checks if can enable submit button: calls isValidated

    const isValidated = () => {
        const passwordValid = $("#password").val().trim();
        return passwordValid && emailValid;
    };               //checks if emailValid was set to 'true' last and also if there's a password
});