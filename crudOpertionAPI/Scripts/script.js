    //Load Data in Table when documents is ready  
    $(document).ready(function () {
        loadData();
    });
    //Load Data function  
    function loadData() {
        $.ajax({
            url: "/Home/List",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
               // debugger
                var data = '';
                $.each(result, function (key,item) {
                    data += '<tr>';
                    data += '<td>' + item.user_id + '</td>';
                    data += '<td>' + item.firstname + '</td>';
                    data += '<td>' + item.lastname + '</td>';
                    data += '<td>' + item.phonenumber + '</td>';
                    data += '<td>' + item.email + '</td>';
                    data += '<td><a href="#" onclick="return getdata(' + item.user_id + ')">Edit</a> | <a href="#" onclick="Delele(' + item.user_id + ')">Delete</a></td>';
                    data += '</tr>';
                });
                $('.tbody').html(data);
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }

//Add Data Function
/*function Add() {
    var res = validate();
    if (res == false) {

        return false;
    }
    var firstname = $("#firstname");
    var lastname = $("#lastname");
    var phonenumber = $("#phonenumber");
    var email = $("#email");
    var password = $("#password");

    $.ajax({
        type: "POST",
        url: "/Home/Add",
        data:
    })
}*/

$(document).ready(function () {
    $("#btnAdd").click(function () {
   //     debugger
        var myformdata = {            
            firstname: $('#firstname').val(),
            lastname: $('#lastname').val(),
            phonenumber: $('#phonenumber').val(),
            email: $('#email').val(),
            password: $('#password').val()
};

        $.ajax({
            type: "POST",
            url: "/Home/Add",
            data: myformdata,
            success: function (responce) {
                if (responce == "success") {
                    alert("User data saved successfully :)")
                }
                else if (responce == 0) {
                    alert("email id already exists");
                } else {
                    return false;
                }
            }
        })

    })
})

   /* function Add() {
        var res = validate();    
        if (res == false) {
            return false;
        }
        console.log(res);   
        var empObj = $("#myForm").serialize();
            *//*{
           // user_id: $('#user_id').val(),
            firstname   : $('#firstname').val(),
            lastname: $('#lastname').val(),
            phonenumber: $('#phonenumber').val(),
            email: $('#email').val(),
            password: $('#password').val()
        };*//*
        $.ajax({
            url: "/Home/Add",
            data: JSON.stringify(empObj),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result == "failed") {
                    return false;
                }
                else { 
                loadData();
                    $('#myModal').modal('hide');
                    }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }*/

//Function for getting the Data Based upon Employee ID

function getdata(id) {
     //debugger
    $('#firstname').css('border-color', 'lightgrey');
    $('#lastname').css('border-color', 'lightgrey');
    $('#phonenumber').css('border-color', 'lightgrey');
    $('#email').css('border-color', 'lightgrey');
    $('#password').css('border-color', 'lightgrey');
     $('#cmppasword').css('border-color', 'lightgrey');
     console.log(id);
     $.ajax({        
         url: "/Home/editdata/",
         data: '{"user_id":"' + parseInt(id) + '"}',
        type: "Get",       
         success: function (result) {
             debugger
             $('#user_id').val(result.user_id);
            $('#firstname').val(result.firstname);
            $('#lastname').val(result.lastname);
            $('#phonenumber').val(result.phonenumber);
             $('#email').val(result.email);
         //    debugger
            $('#password').hide();
            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
     });
     console.log(parseInt(id));
    return false;
}
//function for updating employee's record  
function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var empObj = {
        user_id: $('#user_id').val(),
        firstname: $('#firstname').val(),
        Age: $('#Age').val(),
        State: $('#State').val(),
        Country: $('#Country').val(),
    };
    $.ajax({
        url: "/Home/Update",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#myModal').modal('hide');
            $('#EmployeeID').val("");
            $('#Name').val("");
            $('#Age').val("");
            $('#State').val("");
            $('#Country').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Done
//function for deleting employee's record  
function Delele(user_id) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "/Home/delete/" + user_id,
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                loadData();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes  
function clearTextBox() {
    $('#firstname').val("");
    $('#lastname').val("");
    $('#phonenumber').val("");
    $('#email').val("");
    $('#password').val("");
    $('#cmpassword').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#firstname').css('border-color', 'lightgrey');
    $('#lastname').css('border-color', 'lightgrey');
    $('#phonenumber').css('border-color', 'lightgrey');
    $('#email').css('border-color', 'lightgrey');
}
//Valdidation using jquery  
function validate() {
    var isValid = true;
    if ($('#firstname').val().trim() == "") {
        $('#firstname').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#firstname').css('border-color', 'lightgrey');
    }
    if ($('#lastname').val().trim() == "") {
        $('#lastname').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#lastname').css('border-color', 'lightgrey');
    }
    if ($('#phonenumber').val().trim() == "") {
        $('#phonenumber').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#phonenumber').css('border-color', 'lightgrey');
    }
    if ($('#email').val().trim() == "") {
        $('#email').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#email').css('border-color', 'lightgrey');
    }
    if ($('#password').val().trim() == "") {
        $('#password').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#password').css('border-color', 'lightgrey');
    }
    if ($('#cmpassword').val().trim() == "") {
        $('#cmpassword').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#cmpassword').css('border-color', 'lightgrey');
    }
    return isValid;
}  