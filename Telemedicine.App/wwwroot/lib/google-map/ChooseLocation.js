//----------------------------------------  GeoLocation Code ----------------------------------//
var centerOfMap;
var userLati = document.getElementById("latitude").value;
var userLongi = document.getElementById("longitude").value;
var currentLati = 0; 
var currentLongi = 0;
var map;
var marker = false; ////Has the user plotted their location marker? 
var markers = [];
var zoomLevel = 4; 
var errorMsg = document.getElementById("GeoLocationErrorMsg");

function getLocation() {
    $("#GeoLocationErrorMsg").stop().slideUp();
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition, showError);
    } else {
        $("#GeoLocationErrorMsg").show().delay(3000).fadeOut(1000);
        errorMsg.innerHTML = "Geolocation is not supported by this browser";
    }
}

function initializeValues() {
    userLati = document.getElementById("latitude").value;
    userLongi = document.getElementById("longitude").value;
    initMap(userLati, userLongi);
}
function initializeValuesByMapId(lat, long, mapId) {
    userLati = lat;
    userLongi = long;
    initMapById(userLati, userLongi, mapId);
}
function showPosition(position) {
    currentLati = position.coords.latitude; 
    currentLongi = position.coords.longitude;
    document.getElementById("latitude").value = currentLati; 
    document.getElementById("longitude").value = currentLongi;
    initMap(currentLati, currentLongi);
}

function showError(error) {
    $("#GeoLocationErrorMsg").show().delay(3000).fadeOut(1000);
    switch(error.code) {
        case error.PERMISSION_DENIED:
            errorMsg.innerHTML = "You denied the request for Geolocation"
            break;
        case error.POSITION_UNAVAILABLE:
            errorMsg.innerHTML = "Location information is unavailable"
            break;
        case error.TIMEOUT:
            errorMsg.innerHTML = "The request to get user location timed out"
            break;
        case error.UNKNOWN_ERROR:
            errorMsg.innerHTML = "An unknown error occurred"
            break;
    }
}

//---------------------------------------- Google Map Code ----------------------------------//     

function initMap() {
    if ((userLati == 0 && userLongi == 0) || (userLati == "0" && userLongi == "0")) {
        centerOfMap = new google.maps.LatLng(39.7587363, -98.202164004);
    } else {
        centerOfMap = new google.maps.LatLng(userLati, userLongi);
        zoomLevel = 16;
    }

    if ((currentLati != 0 && currentLongi != 0) && (typeof currentLati !== "undefined") && (typeof currentLongi !== "undefined")){
        centerOfMap = new google.maps.LatLng(currentLati, currentLongi);
        zoomLevel = 16;
    }
 
    var options = {
      center: centerOfMap,
      zoom: zoomLevel 
    };
   
    map = new google.maps.Map(document.getElementById('map'), options);

    marker = new google.maps.Marker({
        position: new google.maps.LatLng(),
        map: map,
        icon: "/lib/google-map/selectedLocationMarker.png",
        title: "Your selected location",
        zIndex: 2,
        draggable: true
    });

    if (userLati != 0 && userLongi != 0) {
        marker.position = centerOfMap;
        markerAddress(userLati, userLongi);
    }

    if(currentLati != 0 && currentLongi != 0){
        var markerImage = new google.maps.MarkerImage('/lib/google-map/CurrentMarker.png',
                new google.maps.Size(50, 50),
                new google.maps.Point(0, 0),
                new google.maps.Point(25, 25));

        var CurrentMarker = new google.maps.Marker({
            position: centerOfMap,
            map: map,
            icon: markerImage,
            title: "Your current location"
        });
    	marker.position = centerOfMap;
        markerAddress(currentLati, currentLongi);
    }

    //------------------------- Listen for any Drag on the marker -----------------------------//.
    if(marker !== false){
        google.maps.event.addListener(marker, 'dragend', function(event){
            markerLocation();
        });    
    }

    //------------------------- Listen for any clicks on the map -----------------------------//.
    google.maps.event.addListener(map, 'click', function(event) { 

        // Remove Previouse marker location that is set by search address
        markers.forEach(function(marker) {
            marker.setMap(null);
        });             

        var clickedLocation = event.latLng;
        marker.setPosition(clickedLocation);
        markerLocation();
    });


    //----------------------------------Search Locations ----------------------------------------//
    // Create the search box and link it to the UI element.
    var input = document.getElementById('pac-input');
    var searchBox = new google.maps.places.SearchBox(input);
    // map.controls[google.maps.ControlPosition.TOP_LEFT].push(input); // it place the search bar on map screen. but give error in current location

    // Bias the SearchBox results towards current map's viewport.
    map.addListener('bounds_changed', function () {
        searchBox.setBounds(map.getBounds());
    });

    // Listen for the event fired when the user selects a prediction and retrieve
    // more details for that place.
    searchBox.addListener('places_changed', getSearchBoxResult);

    function getSearchBoxResult() {
         
        var places = searchBox.getPlaces();

        if (places.length == 0) {
            return;
        }

        // For each place, get the icon, name and location.
        var bounds = new google.maps.LatLngBounds();
        places.forEach(function (place) {
            if (!place.geometry) {
                console.log("Returned place contains no geometry");
                return;
            }

            marker.setPosition(place.geometry.location);

            if (place.geometry.viewport) {
                // Only geocodes have viewport.
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }
        });
        map.fitBounds(bounds);
        markerLocation();
    }
    //-----------------------------------End of Search Locations Code ---------------------------------------------//

}
function initMapById(userLati, userLongi, mapId) {

    if ((userLati == 0 && userLongi == 0) || (userLati == "0" && userLongi == "0")) {
        centerOfMap = new google.maps.LatLng(39.7587363, -98.202164004);
    } else {
        centerOfMap = new google.maps.LatLng(userLati, userLongi);
        zoomLevel = 16;
    }

    if ((currentLati != 0 && currentLongi != 0) && (typeof currentLati !== "undefined") && (typeof currentLongi !== "undefined")) {
        centerOfMap = new google.maps.LatLng(currentLati, currentLongi);
        zoomLevel = 16;
    }

    var options = {
        center: centerOfMap,
        zoom: zoomLevel
    };

    map = new google.maps.Map(document.getElementById(mapId), options);

    marker = new google.maps.Marker({
        position: new google.maps.LatLng(),
        map: map,
        icon: "/lib/google-map/selectedLocationMarker.png",
        title: "Your selected location",
        zIndex: 2,
        draggable: true
    });

    if (userLati != 0 && userLongi != 0) {
        marker.position = centerOfMap;
        markerAddress(userLati, userLongi);
    }

    if (currentLati != 0 && currentLongi != 0) {
        var markerImage = new google.maps.MarkerImage('/lib/google-map/CurrentMarker.png',
            new google.maps.Size(50, 50),
            new google.maps.Point(0, 0),
            new google.maps.Point(25, 25));

        var CurrentMarker = new google.maps.Marker({
            position: centerOfMap,
            map: map,
            icon: markerImage,
            title: "Your current location"
        });
        marker.position = centerOfMap;
        markerAddress(currentLati, currentLongi);
    }

    //------------------------- Listen for any Drag on the marker -----------------------------//.
    if (marker !== false) {
        google.maps.event.addListener(marker, 'dragend', function (event) {
            markerLocation();
        });
    }

    //------------------------- Listen for any clicks on the map -----------------------------//.
    google.maps.event.addListener(map, 'click', function (event) {

        // Remove Previouse marker location that is set by search address
        markers.forEach(function (marker) {
            marker.setMap(null);
        });

        var clickedLocation = event.latLng;
        marker.setPosition(clickedLocation);
        markerLocation();
    });


    //----------------------------------Search Locations ----------------------------------------//
    // Create the search box and link it to the UI element.
    var input = document.getElementById('pac-input');
    var searchBox = new google.maps.places.SearchBox(input);
    // map.controls[google.maps.ControlPosition.TOP_LEFT].push(input); // it place the search bar on map screen. but give error in current location

    // Bias the SearchBox results towards current map's viewport.
    map.addListener('bounds_changed', function () {
        searchBox.setBounds(map.getBounds());
    });

    // Listen for the event fired when the user selects a prediction and retrieve
    // more details for that place.
    searchBox.addListener('places_changed', getSearchBoxResult);

    function getSearchBoxResult() {

        var places = searchBox.getPlaces();

        if (places.length == 0) {
            return;
        }

        // For each place, get the icon, name and location.
        var bounds = new google.maps.LatLngBounds();
        places.forEach(function (place) {
            if (!place.geometry) {
                console.log("Returned place contains no geometry");
                return;
            }

            marker.setPosition(place.geometry.location);

            if (place.geometry.viewport) {
                // Only geocodes have viewport.
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }
        });
        map.fitBounds(bounds);
        markerLocation();
    }
    //-----------------------------------End of Search Locations Code ---------------------------------------------//

}
        
function markerLocation() {

    var currentLocation = marker.getPosition();

    document.getElementById('latitude').value = marker.getPosition().lat();
    document.getElementById('longitude').value = marker.getPosition().lng();

    markerAddress(currentLocation.lat(), currentLocation.lng());
}

function markerAddress(currentLati, currentLongi){
    var google_map_pos = new google.maps.LatLng(currentLati, currentLongi);
    /* Use Geocoder to get address */
    var google_maps_geocoder = new google.maps.Geocoder();
    google_maps_geocoder.geocode(
        { 'latLng': google_map_pos },
        function( results, status ) {
            if ( status == google.maps.GeocoderStatus.OK && results[0] ) {
                document.getElementById('pac-input').value = results[0].formatted_address;
                document.getElementById('address').value = document.getElementById('pac-input').value;
                $('#ChooseLocationModal .modal-error').hide();
            }
        }
    );
}