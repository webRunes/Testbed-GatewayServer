"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/devHub").build();

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (dev, message) {
    //alert("Message Received: " + message + " from " + dev);
    var DevName = "";
    if (dev === "/iot/zolertia/replay") {
        if (message === "on") {
            $("#Mote").addClass("btn-danger");
            $("#Mote").removeClass("btn-success");
            $("#Mote").val("off");
            $("#Mote").html("Turn Off Zolertia");
            $("#Sensor").removeClass("invisible");
            $("#Sensor").addClass("visible");
        }
        if (message === "off") {
            
            $("#Mote").val("on");            
            $("#Mote").addClass("btn-success");
            $("#Mote").removeClass("btn-danger");
            $("#Mote").html("Turn On Zolertia");
            $("#Sensor").addClass("invisible");
            $("#Sensor").removeClass("visible");
            
            $("#Sensor").val("on");
            $("#Sensor").addClass("btn-success");
            $("#Sensor").removeClass("btn-danger");
            $("#Sensor").html("Activate Temperature Sensor");
        }
        DevName = "Zolertia re-mote  ver-b"
    }
    if (dev === "/iot/sensor/replay") {
        if (message === "on") {
            $("#Sensor").addClass("btn-danger");
            $("#Sensor").removeClass("btn-success");
            $("#Sensor").val("off");
            $("#Sensor").html("Deactivate Temperature Sensor");
        }
        if (message === "off") {

            $("#Sensor").val("on");
            $("#Sensor").addClass("btn-success");
            $("#Sensor").removeClass("btn-danger");
            $("#Sensor").html("Activate Temperature Sensor");

        }
        DevName = "Temperature sensor";
    }
    var encodedMsg = DevName + " is " + message.toUpperCase() + " now!!! ";
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    //document.getElementById("messagesList").appendChild(li);    


    $("ul").prepend("<li>" + encodedMsg+"</li>");


});

connection.start().then(function () {
    //document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

/*$('.btn').on('click', function () {
    var btn = $(this);
    btn.html('<span class="spinner-border spinner-border-md" role="status" aria-hidden="true"></span> Loading...'); setTimeout(function () {
        btn.val('reset');
    }, 2000);
});*/

/*$('button[data-loading-text]')
    .on('click', function () {
        var btn = $(this)
        btn.button('loading')
        /*setTimeout(function () {
            btn.button('reset')
        }, 3000)*
    });*/
