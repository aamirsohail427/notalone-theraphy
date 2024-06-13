var name = '';
var loginId = 0;
var videoChatId = 0;
var currUserType = '';
loginId = $('#loginId').val();
videoChatId = $('#videoChatId').val();//videoChatId
name = $('#currUserName').val();

//var API_KEY = '46224112';
var API_KEY = $("#openTokApiKey").val();

var SESSION_ID = $('#sessionId').val();
var TOKEN = $('#tokenId').val();
currUserType = $('#userType').val();
var apiKey,
    session,
    sessionId,
    token,
    response;
var publisher;
var subscriber;
var subscriberNo;
var isFromEndCall = 0;
//-----------------------------------------------------
var isSubscriberOnline = false;
var IsDoctorLogin = $("#IsDoctorLogin").val();
var countDownDatetime = new Date().getTime();
var isCountdownStart = false;
var aptEndDateTime = new Date();
var docAptId = 0;
var expireApt = moment().add(1, 'minutes');
var appointment_id = $("#appointment_id").val();

$(document).ready(function () {
    //debugger;
    GetTodayAppointments();
    console.log("sessionId", SESSION_ID);
    console.log("token", TOKEN);
    console.log("apikey", API_KEY);
    // See the confing.js file.
    if (API_KEY && TOKEN && SESSION_ID) {
        apiKey = API_KEY;
        sessionId = SESSION_ID;
        token = TOKEN;
        initializeSession();
    } else {
        // Make an Ajax request to get the OpenTok API key, session ID, and token from the server
    }

    var scrollHeightControl = $(".messages")[0];
    if (scrollHeightControl != undefined) {
        $("#chatMsgs").stop().animate({ scrollTop: $(".messages")[0].scrollHeight }, 500, 'swing', function () { });
    }
});
//methd to get patient profile
$('#PatientProfileView').click(function () {
    //debugger;
    var patientLoginId = 0;
    patientLoginId = $('#recieverLoginId').val();
    if (patientLoginId != 0) {
        $.ajax({
            type: "GET",
            url: '../Chat/GetPatientProfileByPatientId',
            data: { UserId: patientLoginId },
            beforeSend: function () {
                $("#loadingScreen").css("display", "block");
            },
            success: function (data) {
                $("#loadingScreen").css("display", "none");
                $('.patientProfileSection').html(data);
            },
            error: function (res) {
                $("#loadingScreen").css("display", "none");
            }
        });
    }
});
$('.stopSubscriberVideo').change(function () {
    if ($(this).is(":checked")) {
        subscriber.subscribeToVideo(false);
    } else {
        subscriber.subscribeToVideo(true);
    }
});
$('.userMic').click(function () {
    if ($(this).find('i').hasClass('fa-microphone')) {
        $(this).find('i').removeClass('fa-microphone');
        $(this).find('i').addClass('fa-microphone-slash');

        publisher.publishAudio(false);
    } else {
        $(this).find('i').removeClass('fa-microphone-slash');
        $(this).find('i').addClass('fa-microphone');

        publisher.publishAudio(true);
    }
});
$('.userVideo').click(function () {
    if ($(this).find('i').hasClass('fa-video')) {
        $(this).find('i').removeClass('fa-video');
        $(this).find('i').addClass('fa-video-slash');

        publisher.publishVideo(false);
    }else {
        $(this).find('i').removeClass('fa-video-slash');
        $(this).find('i').addClass('fa-video');
        
        publisher.publishVideo(true);
    }
});

//********************* Call Logs ************************************************
function CallExceptionLogs(Description) {
    var element = "";
    element = { Id: $("#CallLogDetailsId").val(), ChatId: $("#videoChatId").val(), Status: 'Exception', Description: Description, AppointmentId: 0 };
    $.ajax({
        type: "POST",
        url: '../Chat/CallEndedLogs',
        data: { CallLogDetail: element },
        success: function (data) {
        },
        error: function (res) {
            AjaxFailure(res);
        }
    });
}
function CallEndedLogs() {
    //alert($("#videoChatId").val());
    var element = "";
    element = { Id: $("#CallLogDetailsId").val(), ChatId: $("#videoChatId").val(), Status: 'Call ended', Description: 'Call ended by ' + name, AppointmentId: appointment_id };
    
    $.ajax({
        type: "POST",
        url: '../Chat/CallEndedLogs',
        data: { CallLogDetail: element },
        success: function (data) {
        },
        error: function (res) {
            AjaxFailure(res);
        }
    });
}
//********************* End Call Logs ********************************************

$('.terminateCall').click(function () {
    CallEndedLogs();
    //debugger;
    //publisher.publishVideo(false);
    session.signal({
        type: "endcall",
        //  to: _stream.connection,
        data: 'call end'
    },
        function (error) {
            if (error) {
                console.log("signal error: " + error.reason);
            } else {
                console.log("signal sent: begincall:");
            }
        }
    );
});
function exceptionHandler(event) {
    //alertR()
    var Description = "Exception: " + event.code + "::" + event.message;
    CallExceptionLogs(Description)
    console.log("Exception: " + event.code + "::" + event.message);
    //window.close();
}
function saveMessage(SaveLoginId, SaveChatId, SaveMessage, userStatus) {
    $.ajax({
        type: "POST",
        url: '../Chat/SendNewMessage',
        data: { LoginId: SaveLoginId, ChatId: SaveChatId, Message: SaveMessage, UserStatus: userStatus },
        success: function (data) {
        },
        error: function (res) {
            AjaxFailure(res);
        }
    });
}
function initializeSession() {
    var customHeight = +window.innerHeight - 108 + "px";
    session = OT.initSession(apiKey, sessionId);
    OT.on("exception", exceptionHandler);
    // Subscribe to a newly created stream
    session.on('streamCreated', function (event) {
        var subscriberOptions = {
            width: '100%',
            overflow: 'hidden',
            height: customHeight,
            top: '10%',
            position: 'fixed'
        };
        
        subscriber = session.subscribe(event.stream, 'peerVideo', subscriberOptions, function (error) {
            clearInterval(x);
            document.getElementById("timer-label").innerHTML = "Connection Established";
            document.getElementById("timer-label").classList.remove('label-success');
            document.getElementById("timer-label").classList.remove('label-danger');
            document.getElementById("timer-label").classList.add('label-success');
            isSubscriberOnline = true;
            checkCookie();
            //expireApt = moment().add(1, 'minutes');
            var appointmentEndTime = setInterval(function () {
                if (moment().diff(moment(aptEndDateTime), 'minutes') > 0) {
                    document.getElementById("timer-label").innerHTML = "The consultation time of 20 minutes is over, you should complete the consultation.";
                    document.getElementById("timer-label").classList.remove('label-success');
                    document.getElementById("timer-label").classList.remove('label-danger');
                    document.getElementById("timer-label").classList.add('label-danger');
                    clearInterval(appointmentEndTime);
                }
            }, 1000);
            if (error) {
                console.log('There was an error publishing: ', error.name, error.message);
            }

        });

    });

    session.on('sessionDisconnected', function (event) {
        //debugger;
        console.log('You were disconnected from the session.', event.reason);
    });
    session.on('connectionDestroyed', function (event) {
        $("#timer-label").html("Waiting for connection <span class='spinner'><span class='bounce1'></span><span class='bounce2'></span><span class='bounce3'></span></span>");
        document.getElementById("timer-label").classList.remove('label-success');
        document.getElementById("timer-label").classList.remove('label-danger');
        document.getElementById("timer-label").classList.add('label-success');
    });
    session.on("streamPropertyChanged", function (event) {
        //debugger;
        var patientName = $('#patientName').val();
        console.log(event.stream);
        console.log(event.stream.hasAudio);
        if (event.stream.connection.connectionId != session.connection.connectionId) {
            if (!event.stream.hasAudio) {
                //alert(patientName + ' mute the mic');
            }
        }
    });
    // Connect to the session
    session.connect(token, function (error) {
        // If the connection is successful, initialize a publisher and publish to the session
        if (!error) {

            // The client can publish. See the next section.
            var publisherOptions = {

            };

            publisher = OT.initPublisher('myVideo', publisherOptions, function (error) {
                if (error) {
                    console.log('There was an error initializing the publisher: ', error.name, error.message);
                    return;
                }
                session.publish(publisher, function (error) {
                    if (error) {
                        console.log('There was an error publishing: ', error.name, error.message);
                    } else {
                        $('#myVideo').css('border-radius', '20px');
                        if ($('#chatType').val() == 'audio') {
                            publisher.publishVideo(false);
                            $('.userVideo').prop('checked', true);
                        }
                        publisher.on({
                            streamDestroyed: function (event) {
                                if (event.reason === 'networkDisconnected') {
                                    alert('Your publisher lost its connection. Please check your internet connection and try publishing again.');
                                }
                            }
                        });
                    }

                });
            });


        } else {
            //debugger;
            console.log('There was an error connecting to the session: ', error.name, error.message);
        }
    });

    session.on("signal", signalEventHandler);
}
//signal event handler
function signalEventHandler(event) {
    if (event.type == 'signal:msg') {
        //debugger;
        //var msg = document.createElement('p');
        var msgData = event.data;
        console.log(msgData);
        var currDate = moment().format('MMM Do, h:mm a');
        //var msg = "<p>" + msgData.msg + "</p>";
        if (loginId != msgData.loginId) {
            var chatMessage = '<li class="incoming"><div class="chat-panel"><div class="sender text-left">' + msgData.userName + '</div><br /><div class="message-data text-left">' + msgData.msg + '</div><br /><div class="time text-right">' + currDate + '</div></div></li>';
            $('.messages').append(chatMessage);
            if (!$('#chat').hasClass('show')) {
                $('#chat').addClass('show');

                $('.VideoControl').css({
                    width: '50%',
                    float: 'left',
                    transition: '0.5s ease-in'

                });
                $('.feedbackbtn').css({
                    left: '-20%'

                });
                $('#myVideo').css({
                    'margin-left': '0px'
                });

                $('.bluescaleM').show();
                $('.bluescaleP, .greyscalM, .bluescale').hide();




            }
            $('#msgTone')[0].play();
            setTimeout(function () {
                $('#msgTone')[0].pause();
                $('#msgTone')[0].currentTime = 0;
                var scrollHeightControl = $(".messages")[0];
                if (scrollHeightControl != undefined) {
                    $("#chatMsgs").stop().animate({ scrollTop: $(".messages")[0].scrollHeight }, 500, 'swing', function () { });
                }
                // Do something after 1 second
            }, 1000);
        }
    }
    if (event.type == 'signal:endcall') {
        if (currUserType != '') {
            if (currUserType.toLowerCase() == 'patient') {
                isFromEndCall = 1;
                $('#chatFeedbackModal').modal('show');
            } else {
                window.location.href = '../DoctorDashboard/DashboardHome';
                //window.close();
            }
        }
        //window.close();
    }
}

$("form").removeData("validator");
$("form").removeData("unobtrusiveValidation");
$.validator.unobtrusive.parse("form");

function check(ele) {
    if (event.key === 'Enter') {
        var msg = '';
        var currDate = moment().format('MMM Do, h:mm a');
        var trimmedValue = jQuery.trim(ele.value);
        if (trimmedValue.length <= 0) {
            // TODO
            return;
        }
        msg = ele.value;
        saveMessage(loginId, videoChatId, msg, 1);
        var chatMessage = '<li class="yourself"><div class="chat-panel"><div class="sender text-left">' + name + '</div><br /><div class="message-data text-left">' + msg + '</div><br /><div class="time text-right">' + currDate + '</div></div></li>';
        $('.messages').append(chatMessage);
        var scrollHeightControl = $(".messages")[0];
        if (scrollHeightControl != undefined) {
            $("#chatMsgs").stop().animate({ scrollTop: $(".messages")[0].scrollHeight }, 500, 'swing', function () { });
        }
        var customObj = { 'msg': msg, 'loginId': loginId, 'userName': name }
        session.signal({
            type: 'msg',
            data: customObj
        }, function (error) {
            if (error) {
                console.log('Error sending signal:', error.name, error.message);
            } else {
                // msgTxt.value = '';
            }
        });

        $('#msgText').val('');
    }

}
$('#sendTxt').click(function () {
    var msg = '';
    msg = $('#msgText').val();
    var currDate = moment().format('MMMM Do YYYY, h:mm:ss a');
    var trimmedValue = jQuery.trim(msg);
    if (trimmedValue.length <= 0) {
        // TODO
        return;
    }
    saveMessage(loginId, videoChatId, msg, 1);
    var chatMessage = '<li class="yourself"><div class="chat-panel"><div class="sender text-left">' + name + '</div><br /><div class="message-data text-left">' + msg + '</div><br /><div class="time text-right">' + currDate + '</div></div></li>';
    $('.messages').append(chatMessage);
    var scrollHeightControl = $(".messages")[0];
    if (scrollHeightControl != undefined) {
        $("#chatMsgs").stop().animate({ scrollTop: $(".messages")[0].scrollHeight }, 500, 'swing', function () { });
    }
    var customObj = { 'msg': msg, 'loginId': loginId, 'userName': name }
    session.signal({
        type: 'msg',
        data: customObj
    }, function (error) {
        if (error) {
            console.log('Error sending signal:', error.name, error.message);
        } else {
            // msgTxt.value = '';
        }
    });

    $('#msgText').val('');
});
$('#messagealert').click(function () {
    //debugger;
    if ($('#chat').hasClass('show')) {
        $('#chat').removeClass('show');
        $('#sendTxt').hide();
        $('.VideoControl').css({
            width: '100%',
            float: 'left',
            transition: '0.5s ease-in'

        });
        $('#myVideo').css({
            'margin-left': '30%',
            'left': '0'
        });
        $('.bluescaleM').hide();
        $('.greyscalP, .greyscal, .greyscalM,.greyscalEHR').show();
    }
    else {
        $('#chat').addClass('show');
        $('#ProfileViewPatient').removeClass('show');
        $('#AddPrescrip').removeClass('show');
        $('#sec_ehr').removeClass('show');
        $('#sec_orderlist').removeClass('show');
        //$('#sendTxt').show(); tayyab
        $('.VideoControl').css({
            width: '50%',
            float: 'left',
            transition: '0.5s ease-in'

        });
        $('.feedbackbtn').css({
            left: '-20%'

        });


        $('#myVideo').css({
            'margin-left': '0px',
            'left': '0'
        });
        $('.bluescaleM').show();
        $('.bluescale, .bluescaleP, .greyscalM,.bluescaleEHR').hide();
        $('.greyscal, .greyscalP,.greyscalEHR').show();



    }

});
// CLose Chat
$('#closeChat').click(function () {
    $('#chat').removeClass('show');
    $('#ProfileViewPatient').removeClass('show');
    $('#AddPrescrip').removeClass('show');
    $('#sec_ehr').removeClass('show');
    $('#sendTxt').hide();
    $('.VideoControl').css({
        width: '100%',
        float: 'left',
        transition: '0.5s ease-in'

    });
    $('#myVideo').css({
        'margin-left': '30%',
        'left': '0'
    });
    $('.bluescaleM').hide();
    $('.greyscaleP, .greyscal, .greyscalM,.greyscalEHR').show();

});
//CloseViewProfile 

$('#closeprofile').click(function () {
    $('#chat').removeClass('show');
    $('#ProfileViewPatient').removeClass('show');
    $('#AddPrescrip').removeClass('show');
    $('#sec_ehr').removeClass('show');
    $('#sendTxt').hide();
    $('.VideoControl').css({
        width: '100%',
        float: 'left',
        transition: '0.5s ease-in'

    });
    $('#myVideo').css({
        'margin-left': '30%',
        'left': '0'
    });
    $('.bluescale').hide();
    $('.greyscalP, .greyscal, .greyscalM,.greyscalEHR').show();



});
//make


$('#closeprescription').click(function () {
    $('#chat').removeClass('show');
    $('#ProfileViewPatient').removeClass('show');
    $('#AddPrescrip').removeClass('show');
    $('#sec_ehr').removeClass('show');
    $('#sendTxt').hide();
    $('.VideoControl').css({
        width: '100%',
        float: 'left',
        transition: '0.5s ease-in'

    });
    $('#myVideo').css({
        'margin-left': '30%',
        'left': '0'
    });
    $('.bluescaleP').hide();
    $('.greyscalP, .greyscal, .greyscalM,.greyscalEHR').show();

});





$('#chatmodalShow').click(function () {
    isFromEndCall = 0;
    $('#chatFeedbackModal').modal('show');
    $('div').removeClass('modal-backdrop');
});


$('#PatientProfileView').click(function () {
    ////debugger;
    if ($('#ProfileViewPatient').hasClass('show')) {
        $('#ProfileViewPatient').removeClass('show');
        $('.VideoControl').css({
            width: '100%',
            float: 'left',
            transition: '0.5s ease-in'

        });
        $('#myVideo').css({
            'margin-left': '30%',
            'left': '0'
        });
        $('.bluescale').hide();
        $('.greyscal, .greyscalP, .greyscalM,.greyscalEHR').show();

    }
    else {

        $('#ProfileViewPatient').addClass('show');
        $('#chat').removeClass('show');
        $('#AddPrescrip').removeClass('show');
        $('#sec_ehr').removeClass('show');
        $('#sec_orderlist').removeClass('show');

        $('#sendTxt').hide();
        $('.VideoControl').css({
            width: '50%',
            float: 'left',
            transition: '0.5s ease-in'

        });
        $('.feedbackbtn').css({
            left: '-20%'

        });
        $('#myVideo').css({
            'margin-left': '0px',
            'left': '0'
        });
        $('.bluescale').show();
        $('.greyscal, .bluescaleM, .bluescaleP,.bluescaleEHR').hide();
        $('.greyscalM, .greyscalP,.greyscalEHR').show();

    }

});
$('.web-view-browser').click(function () {
    if ($('#sec_ehr').hasClass('show')) {
        $('#sec_ehr').removeClass('show');
        $('#sendTxt').hide();
        $('.VideoControl').css({
            width: '100%',
            float: 'left',
            transition: '0.5s ease-in'

        });
        $('#myVideo').css({
            'margin-left': '30%',
            'left': '0'
        });
        $('.bluescaleEHR').hide();
        $('.greyscalP, .greyscal, .greyscalM,.greyscalEHR').show();
    }
    else {
        $('#sec_ehr').addClass('show');
        $('#chat').removeClass('show');
        $('#ProfileViewPatient').removeClass('show');
        $('#AddPrescrip').removeClass('show');
        $('#sendTxt').hide();
        $('.VideoControl').css({
            width: '50%',
            float: 'left',
            transition: '0.5s ease-in'

        });
        $('.feedbackbtn').css({
            left: '-20%'

        });


        $('#myVideo').css({
            'left': '-130px',
            'margin-left': '0'
        });

        $('.bluescaleEHR').show();
        $('.bluescale, .bluescaleP,.bluescaleM, .greyscalEHR').hide();
        $('.greyscal, .greyscalP,.greyscalM').show();
    }
});

$('#closeEhr').click(function () {
    $('#chat').removeClass('show');
    $('#ProfileViewPatient').removeClass('show');
    $('#AddPrescrip').removeClass('show');
    $('#sec_ehr').removeClass('show');
    $('#sendTxt').hide();
    $('.VideoControl').css({
        width: '100%',
        float: 'left',
        transition: '0.5s ease-in'

    });
    $('#myVideo').css({
        'margin-left': '30%',
        'left': '0'
    });
    $('.bluescaleEHR').hide();
    $('.greyscalP, .greyscal, .greyscalM,.greyscalEHR').show();



});
$(".addPrescription").click(function () {
    /*var patientProfileId = $('#recieverLoginId').val();
    var id = 0;
    var visitId = 0;
    var isEdit = false;
    var isFirstTime = true;*/

    patientProfileId = $('#recieverProfileId').val();
    if ($('#AddPrescrip').hasClass('show')) {
        $('#AddPrescrip').removeClass('show');
        $('.VideoControl').css({
            width: '100%',
            float: 'left',
            transition: '0.5s ease-in'

        });
        $('#myVideo').css({
            'margin-left': '30%'
        });
        $('.greyscalP').show();
        $('.bluescaleP, .bluescale, .bluescaleM').hide();

    } else {

        $('#AddPrescrip').addClass('show');
        $('#chat').removeClass('show');
        $('#ProfileViewPatient').removeClass('show');
        $('#sec_ehr').removeClass('show');
        $('#sec_orderlist').removeClass('show');
        $('.VideoControl').css({
            width: '50%',
            float: 'left',
            transition: '0.5s ease-in'

        });
        $('.feedbackbtn').css({
            left: '-20%'

        });
        $('#myVideo').css({
            'margin-left': '0px'
        });
        $('.greyscalP, .bluescaleM, .bluescale,.bluescaleEHR').hide();
        $('.bluescaleP, .greyscalM, .greyscal,.greyscalEHR').show();
        $("#loadingScreen").css("display", "none");
        $("form").removeData("validator");
        $("form").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse("form");

    }
});
$('#vidOrderList').click(function () {

    if ($('#sec_orderlist').hasClass('show')) {
        $('#sec_orderlist').removeClass('show');
        $('#vidOrderList #sendTxt').hide();
        $('.VideoControl').css({
            width: '100%',
            float: 'left',
            transition: '0.5s ease-in'

        });
        $('#myVideo').css({
            'margin-left': '30%',
            'left': '0'
        });
        $('.bluescaleEHR').hide();
        $('.greyscalP, .greyscal, .greyscalM,.greyscalEHR').show();
    }
    else {

        $('#sec_ehr').removeClass('show');
        $('#sec_orderlist').addClass('show');
        $('#chat').removeClass('show');
        $('#ProfileViewPatient').removeClass('show');
        $('#AddPrescrip').removeClass('show');


        $('#sendTxt').hide();
        $('.VideoControl').css({
            width: '50%',
            float: 'left',
            transition: '0.5s ease-in'

        });
        $('.feedbackbtn').css({
            left: '-20%'

        });


        $('#myVideo').css({
            'left': '-130px',
            'margin-left': '0'
        });

        $('.bluescaleEHR').show();
        $('.bluescale, .bluescaleP,.bluescaleM, .greyscalEHR').hide();
        $('.greyscal, .greyscalP,.greyscalM').show();
    }
});

$('#closeOrderLists').click(function () {
    $('#chat').removeClass('show');
    $('#ProfileViewPatient').removeClass('show');
    $('#AddPrescrip').removeClass('show');
    $('#sec_orderlist').removeClass('show');
    $('#sendTxt').hide();
    $('.VideoControl').css({
        width: '100%',
        float: 'left',
        transition: '0.5s ease-in'

    });
    $('#myVideo').css({
        'margin-left': '30%',
        'left': '0'
    });
    $('.bluescaleEHR').hide();
    $('.greyscalP, .greyscal, .greyscalM,.greyscalEHR').show();



});

//------------------------ Timer -------------------------

function GetTodayAppointments() {
    $.get("../../Patient/GetTodayAppointment", function (data) {
        jQuery(data).each(function (i, item) {
            var appointmentStartDateTime = moment(item.appointmentDate).format("MMMM DD, YYYY") + " " + moment(item.startTime).format("hh:mm:ss A");
            aptEndDateTime = moment(item.appointmentDate).format("MMMM DD, YYYY") + " " + moment(item.endTime).format("hh:mm:ss A");
            docAptId = item.id;
            var remainingMinutes = 10 - moment().diff(moment(appointmentStartDateTime), 'minutes');
            if (remainingMinutes >= 0 && remainingMinutes <= 10) {
                //countDownDatetime = new Date(moment().add(remainingMinutes, 'minutes').format("MMMM DD, YYYY hh:mm:ss A")).getTime();
                countDownDatetime = moment(appointmentStartDateTime).add(10,'minutes').valueOf();
                isCountdownStart = true;
            }
            //if (IsDoctorLogin == "true") {
            //    GetPatientAllergies(item.userId);
            //}
        });
    });
}


var x = setInterval(function () {
    var cookie = getCookie("sessionId");
    if (isSubscriberOnline === false && isCountdownStart === true && cookie === "") {
        CountdownTimer();
    } else {
        if (cookie === SESSION_ID) {
            $("#timer-label").html("Waiting for connection <span class='spinner'><span class='bounce1'></span><span class='bounce2'></span><span class='bounce3'></span></span>");
            //var waitForConnection = $("#timer-label").html("Waiting for connection <span class='spinner'><span class='bounce1'></span><span class='bounce2'></span><span class='bounce3'></span></span>");
            //document.getElementById("timer-label") = waitForConnection;
            //document.getElementById("timer-label").innerHTML = "waiting for connection...";
        } else {
            //document.getElementById("timer-label").innerHTML = "session is changed";
        }
    }
}, 1000);

function CountdownTimer() {
    // Get todays date and time
    //var now = new Date().getTime();
    var now = moment().valueOf();

    // Find the distance between now and the count down date
    var distance = countDownDatetime - now;
    // Time calculations for days, hours, minutes and seconds
    var days = Math.floor(distance / (1000 * 60 * 60 * 24));
    var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
    var seconds = Math.floor((distance % (1000 * 60)) / 1000);

    // Display the result in the element with id="demo"
    if (minutes == 0 || minutes == 1) {
        document.getElementById("timer-label").innerHTML = minutes + " minute " + seconds + " seconds left";
    } else {
        document.getElementById("timer-label").innerHTML = minutes + " minutes " + seconds + " seconds left";
    }


    // If the count down is finished, write some text 
    if (distance < 0) {
        clearInterval(x);
        document.getElementById("timer-label").innerHTML = "Expired";
        if (IsDoctorLogin == "true") {
            window.location.href = "../DoctorDashboard/DashboardHome";
        }
        else {
            CancelSloatORPatientFunction(docAptId);

        }
    }
}
function createCookie(name, value, minutes) {
    var expires = "; ";
    if (minutes) {
        var date = new Date();
        date.setTime(date.getTime() + (minutes * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    }
    document.cookie = name + "=" + value + expires + "; path=/";
}
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
function checkCookie() {
    var cookie = getCookie("sessionId");
    if (cookie != "") {
        console.log(cookie);
    } else {
        cookie = SESSION_ID;
        if (cookie != "" && cookie != null) {
            createCookie("sessionId", cookie,20);
        }
    }
}
function CancelSloatORPatientFunction(DocAptId) {
    $.ajax({
        type: "Post",
        url: '/DoctorDashboard/CancelPatientAppointmentSlot',
        data: { DoctorAppointmentId: DocAptId },
        beforeSend: function () {
        },
        success: function (data) {
            $("#modal_cancel_patient_appointment").modal("show");
        },
        error: function (res) {
            AjaxFailure(res);
        }
    });
}
function ModalCancelAppointment() {
    $("#modal_cancel_patient_appointment").modal("hide");
    window.location.href = "../Patient/Insights";
}
