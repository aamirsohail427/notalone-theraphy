// replace these values with those generated in your TokBox Account
var apiKey = document.getElementById('ApiKey').value;
var sessionId = document.getElementById('SessionId').value;
var token = document.getElementById('Token').value;
var publisher = null;
var subscriber = null;
// (optional) add server code here
initializeSession();

// Handling all of our errors here by alerting them
function handleError(error) {
    if (error) {
        alert(error.message);
    }
}

function initializeSession() {
    var session = OT.initSession(apiKey, sessionId);

    // Subscribe to a newly created stream
    session.on('streamCreated', function (event) {
        subscriber =  session.subscribe(event.stream, 'subscriber', {
            insertMode: 'append',
            width: '100%',
            height: '100%'
        }, handleError);
    });

    // Create a publisher
    publisher = OT.initPublisher('publisher', {
        insertMode: 'append',
        width: '100%',
        height: '100%'
    }, handleError);

    // Connect to the session
    session.connect(token, function (error) {
        // If the connection is successful, publish to the session
        if (error) {
            handleError(error);
        } else {
            session.publish(publisher, handleError);
        }
    });
}
function micSwitch(inputElement) {
    if ($(inputElement).closest("button").hasClass('btn-success') === true) {
        $(inputElement).closest("button").addClass('btn-danger').removeClass('btn-success');
        publisher.publishAudio(false);
        return;
    }

    if ($(inputElement).closest("button").hasClass('btn-danger') === true) {
        $(inputElement).closest("button").addClass('btn-success').removeClass('btn-danger');
        publisher.publishAudio(true);
        return;
    }
}
function videoSwitch(inputElement) {
    if ($(inputElement).closest("button").hasClass('btn-success') === true) {
        $(inputElement).closest("button").addClass('btn-danger').removeClass('btn-success');
        publisher.publishVideo(false);
        return;
    }

    if ($(inputElement).closest("button").hasClass('btn-danger') === true) {
        $(inputElement).closest("button").addClass('btn-success').removeClass('btn-danger');
        publisher.publishVideo(true);
        return;
    }
}
function discountSession() {
    session.discount();
}