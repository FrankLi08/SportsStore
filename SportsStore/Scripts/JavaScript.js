

function checkfunction()
{
    var uname= $('#username').val();
    $.ajax(
        {
            type: 'POST',
           
            url: 'checkusername',
            data: {username:uname },
            dataType: 'json',
            success: function (data) {
                if (data === "") {
                    alert("Username already exist");
                    $('#username').val("").focus();

                }
            }


        });
}

                           


