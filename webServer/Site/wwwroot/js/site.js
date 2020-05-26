// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
class User {

    contructor() {
        this.login = "";
        this.password = "";
    }
}

var user = null;

function checkUserOnServer(user) {
    fetch('http://localhost:49961/api/User/' + user.login + '/' + user.password)
        .then((response) => {
            return response.json();
        })
        .then((data) => {
            console.log(data);
            if (data) {
                var str = user.login + "=" + user.password;
                document.cookie = str;
                updateInterface();
            }
            else {
                user = null;
                document.cookie = "";
                updateInterface();
            }
            return data;
        });
}
//login=admin; password=Pa$$w0rd
function getUserFromCookie() {
    var cookie = document.cookie;
    var arr = cookie.split("=");
    console.log(arr);
    if (arr.length == 2 ) {
        user = new User();
        user.login = arr[0];
        user.password = arr[1];
        checkUserOnServer(user);
    }
    else {
        user = null;
    }
}

function updateInterface() {
    var divSigned = document.getElementById("divSigned");
    var divSignIn = document.getElementById("divSignIn");
    var lbLogin = document.getElementById("lbLogin");
    if (user == null) {
        divSigned.style.visibility = "hidden";
        divSigned.style.display = "none";
        divSignIn.style.visibility = "visible";
        divSignIn.style.display = "inline-block";
    }
    else {
        divSigned.style.visibility = "visible";
        divSigned.style.display = "inline-block";
        divSignIn.style.visibility = "hidden";
        divSignIn.style.display = "none";

        lbLogin.innerHTML = user.login;
    }
} 

ymaps.ready(init);


function getColor(value) {
    if (value >= 0 && value < 0.1) {
        return '#baddb4';
    }
    if (value < 0.15) {
        return '#ffff92';
    }
    if (value < 0.2) {
        return '#f6dbd0';
    }
    if (value < 0.25) {
        return '#feb7a1';
    }
    return '#f4995a';
}

function LoadDataFromServer() {
    fetch('http://localhost:49961/api/Note')
        .then((response) => {
            return response.json();
        })
        .then((data) => {
            data.forEach(function (entry) {
                console.log(entry);
                console.log(entry.latitude);
                var myPlacemark = new ymaps.Placemark([entry.latitude, entry.longtitude], {

                    balloonContentHeader: entry.indication,
                    balloonContentBody: entry.dateTime.split("T").join(" "),
                    balloonContentFooter: "Долгота: " + entry.longtitude + ", " + "Широта: " + entry.latitude,
                    hintContent: entry.indication
                },
                    {
                        preset: "islands#circleDotIcon",
                        // Задаем цвет метки (в формате RGB).
                        iconColor: getColor(entry.indication)
                    });
                myMap.geoObjects.add(myPlacemark);
            });
            console.log(data);
        });
}


var myMap = null;

function init() {
    getUserFromCookie();
    updateInterface();

     myMap = new ymaps.Map("map", {
        center: [54.83, 37.11],
        zoom: 5
    }, {
        searchControlProvider: 'yandex#search'
    });

    LoadDataFromServer();
    //var request = new XMLHttpRequest();
    //request.open('GET', "http://localhost:49961/api/Note");
    //var result = "";
    //request.responseType = 'text';

    //request.onload = function () {
    //    result = request.response;
    //   var myPlacemark = new ymaps.Placemark([55.907228, 31.260503], {
    //        // Чтобы балун и хинт открывались на метке, необходимо задать ей определенные свойства.
    //        balloonContentHeader: "Балун метки",
    //        balloonContentBody: result,
    //        balloonContentFooter: "Подвал",
    //        hintContent: "Хинт метки"
    //    });
    //    myMap.geoObjects.add(myPlacemark);
    //};

    //request.send();
}

function SignIn() {
    var login = document.getElementById("inputLogin").value;
    var password = document.getElementById("inputPass").value;
    if (login == "" || password == "")
        alert("Введите логин и пароль!");
    else {
        user = new User();
        user.login = login;
        user.password = password;
        checkUserOnServer(user);
    }
}

function SignOut() {
    user = null;
    document.cookie = "";
    updateInterface();
}

class Note {
    constructor() {
        this.ID = 0;
        this.DateTime = "";
        this.Longtitude = 0;
        this.Latitude = 0;
        this.Indication = 0;
    }
} 


async function postData(url = '', data = {}) {
    // Default options are marked with *
    const response = await fetch(url, {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *client
        body: JSON.stringify(data) // body data type must match "Content-Type" header
    });
    return await response.json(); // parses JSON response into native JavaScript objects
}

function LoadDataFromFile(path) {
    var fr = new FileReader();
    try {
        fr.onload = function () {
            var text = fr.result;

            var lines = text.split("\n");
            var elems = [];
            for (var i = 0; i < lines.length; ) {
                var note = new Note();
                note.DateTime = lines[i];
                note.Indication = lines[i + 1];
                note.Latitude = lines[i + 2];
                note.Longtitude = lines[i + 3];
                i += 4;
                elems.push(note);
            }

            console.log(elems);

            postData('http://localhost:49961/api/Note/', elems).then((data) => {
                console.log(data); // JSON data parsed by `response.json()` call
                if (data) {
                    LoadDataFromServer();
                }
            });

            document.getElementById("inputFile").value = null; 
        }
        fr.readAsText(path); 
    } catch (e) {
        console.error(err);
    }
}

function LoadFile() {
    var path = document.getElementById("inputFile").files[0]; 
    if (path == null || path == "")
        alert("Файл не выбран!");
    LoadDataFromFile(path);    
}