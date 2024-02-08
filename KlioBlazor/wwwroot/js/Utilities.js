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

function redirectToUrl(url) {
    window.location.replace(url);
}

function initFlickity() {
    var elem1 = document.querySelector('.staff-carousel');
    var flkty1 = new Flickity(elem1, {
        // options
        cellAlign: 'left',
        contain: true,
        freeScroll: true,
        prevNextButtons: true,
        pageDots: false,
        groupCells: false
    });
    var elem2 = document.querySelector('.actor-carousel');
    var flkty2 = new Flickity(elem2, {
        // options
        cellAlign: 'left',
        contain: true,
        freeScroll: true,
        prevNextButtons: true,
        pageDots: false,
        groupCells: false
    });
    var elem3 = document.querySelector('.transstaff-carousel');
    var flkty3 = new Flickity(elem3, {
        // options
        cellAlign: 'left',
        contain: true,
        freeScroll: true,
        prevNextButtons: true,
        pageDots: false,
        groupCells: false
    });
    var elem4 = document.querySelector('.transactor-carousel');
    var flkty4 = new Flickity(elem4, {
        // options
        cellAlign: 'left',
        contain: true,
        freeScroll: true,
        prevNextButtons: true,
        pageDots: false,
        groupCells: false
    });
}