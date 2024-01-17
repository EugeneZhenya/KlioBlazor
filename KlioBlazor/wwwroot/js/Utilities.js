function my_function(message) {
    console.log("from utilities " + message);
}

function dotnetStaticInvocation() {
    DotNet.invokeMethodAsync("KlioBlazor", "GetCurrentCount")
        .then(result => {
            console.log("count from javascript " + result);
        });
}

function dotnetInstanceInvocation(dotnetHelper) {
    dotnetHelper.invokeMethodAsync("AddNewMovie");
}

function mobMenuVisible() {
    $('body').addClass('mobile-menu-visible');
}

function mobMenuUnisible() {
    $('body').removeClass('mobile-menu-visible');
}

function mobDropdownToggle(element) {
    $('#' + element).toggleClass('open');
    $('#' + element).prev('ul').slideToggle(500);
}

function playerInit(id, file, poster, title, autoplay) {
    console.log('player: ' + file );
    var player = new Playerjs({ id: id, file: file, poster: poster, title: title, autoplay: autoplay });
}

function playerStop(id, file, poster, title, autoplay) {
    console.log('player: ' + id + ' stop...');
    var player = new Playerjs({ id: id, file: file, poster: poster, title: title, autoplay: autoplay });
    player.api("stop");
}