
var apiKey = $('#chat-credentials').attr('data-api-key');
var sessionId = $('#chat-credentials').attr('data-session-id');
var tokenId = $('#chat-credentials').attr('data-token-id');
var userId = $('#chat-credentials').attr('data-user-id');
var fromUserName = $('#chat-credentials').attr('data-user-name');
var toUserName = '';
var toUserId = 0;
var parentId = 0;


$(document).ready(function () {  
    connect(sessionId, tokenId);
});

function connect(sessionId, token) {
    OT.on("exception", exceptionHandler);


    if (!(OT.checkSystemRequirements())) {
        alert("You don't have the minimum requirements to run this application.");
    } else {

        session = OT.initSession(sessionId);	// Initialize session
        session.connect(apiKey, token);
        // Add event listeners to the session
        session.on('sessionConnected', sessionConnectedHandler);
        session.on('sessionDisconnected', sessionDisconnectedHandler);
        session.on('connectionCreated', connectionCreatedHandler);
        session.on('connectionDestroyed', connectionDestroyedHandler);
        session.on("signal", signalEventHandler);
    }
}


function sessionConnectedHandler(event) {

    console.log('session connected');
}

//********************* End Call Logs ********************************************
function signalEventHandler(event) {

    if (event.type == 'signal:msg') {
        //{ FromUserId: userId, ToUserId: toUserId, Message: message, IsRead: false, ParentId: parentId }
        var msgObj = JSON.parse(event.data);
        //var msgObj = event.data;
        if (msgObj.ToUserId == userId) {
            
            if ($('.chat-user[data-to-user-id=' + msgObj.FromUserId + ']').hasClass('active')) {
                var html = '<div class="chat"> <div class="chat-avatar">' +
                    '<a class="avatar">' +
                    '<img src="#" onerror="transformErrorImgToLettersAvatar(this,\'' + msgObj.FromUserName + '\', \'40\')" class="circle" alt="avatar" />' +
                    '</a>' +
                    '</div >' +
                    '<div class="chat-body">' +
                    '<div class="chat-text">' +
                    '<p>' + msgObj.Message + '</p>' +
                    '</div>' +
                    '</div>' +
                    '</div >'

                $(".chats").append(html);
                $(".chat-area").scrollTop($(".chat-area > .chats").height());
            }
        }
      
    }
    else if (event.type == 'signal:notification') {
        debugger;
        var msgObj = JSON.parse(event.data);
        if (msgObj.ToUserId == userId) {
            getNotifications();
            M.toast({ html: msgObj.Message, classes: 'teal' });
        }
    }
}


function sessionDisconnectedHandler(event) {

    console.log('session disconnected');
    session.off('sessionConnected', sessionConnectedHandler);
    session.off('streamCreated', streamCreatedHandler);
    session.off('streamDestroyed', streamDestroyedHandler);
    session.off('connectionCreated', connectionCreatedHandler);
    session.off("signal", signalEventHandler);
    OT.off("exception", exceptionHandler);
    session.off('sessionDisconnected', sessionDisconnectedHandler);


}

function saveMessage(SaveLoginId, SaveChatId, SaveMessage, userStatus, fileName, fileType, fileSize) {
    $.ajax({
        type: "POST",
        url: '../Chat/SendNewMessage',
        data: { LoginId: SaveLoginId, ChatId: SaveChatId, Message: SaveMessage, UserStatus: userStatus, FileName: fileName, FileType: fileType, Filesize: fileSize },
        success: function (data) {
        },
        error: function (res) {
            AjaxFailure(res);
        }
    });
}
function getchatList(loginId) {
    $.ajax({
        type: "GET",
        url: '../Chat/GetChatListByLoginId',
        data: { loginId: loginId },
        success: function (data) {
            if (data.length > 0) {
                chatList = data;
            }
        },
        error: function (res) {
            AjaxFailure(res);
        }
    });
}

function connectionDestroyedHandler(event) {

    console.log('connection destroyed');
}


function connectionCreatedHandler(event) {
    console.log('connection created');
}

/*
If you un-comment the call to OT.setLogLevel(), above, OpenTok automatically displays exception event messages.
*/
function exceptionHandler(event) {
    console.log("Exception: " + event.code + "::" + event.message);
}

//--------------------------------------
//  HELPER METHODS
//--------------------------------------



// Add message to chat
function enter_chat(source) {
    var message = $(".message").val();
    if (message != "") {
        var html = '<div class="chat chat-right"> <div class="chat-avatar">' +
            '<a class="avatar">' +
            '<img src="#" onerror="transformErrorImgToLettersAvatar(this, \'' + fromUserName + '\', \'40\')" class="circle" alt="avatar" />' +
            '</a>' +
            '</div >' +
            '<div class="chat-body">' +
            '<div class="chat-text">' +
            '<p>' + message + '</p>' +
            '</div>' +
            '</div>' +
            '</div >'

        $(".chats").append(html);
        $(".message").val("");
        $(".chat-area").scrollTop($(".chat-area > .chats").height());
        sendMessage(message);

    }
}

function sendBookedAppointmentNotification(doctorId) {
    var message = fromUserName + " booked your appointment";
    var jsonObj = { FromUserId: userId, FromUserName: fromUserName, ToUserId: doctorId, Message: message};
    var customJson = JSON.stringify(jsonObj);
    setTimeout(
        function () {
            sendSignal(customJson, 'notification');        
        }, 8000);

}

function sendMessage(message) {

    toUserId = $('.sender-detail').attr('data-to-user-id');
    parentId = $('.sender-detail').attr('data-parent-id');
    var jsonObj = { FromUserId: userId, FromUserName: fromUserName, ToUserId: toUserId, Message: message, IsRead: false, ParentId: parentId };
    var customJson = JSON.stringify(jsonObj);
    sendSignal(customJson, 'msg');
    saveMessage(jsonObj);
}

function saveMessage(jsonObj) {
    $.ajax({
        type: "POST",
        beforeSend: function(xhr) {
            $("#ajax_preloader").hide();
        },
        url: '../Conversations/Save',
        data: { dto: jsonObj },
        success: function (data) {

        }
    });
}

function sendSignal(customJson, signalType) {
    
    session.signal({
        type: signalType,
        data: customJson
    }, function (error) {
        if (error) {
            console.log('Error sending signal:', error.name, error.message);
        }
    });
}