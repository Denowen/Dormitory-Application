window.onload = function(){
    var slideImage = document.getElementById("slider-img");
    var images = ["/img/Turuncu-Genel.jpg","/img/Turuncu-Genel2.jpg","/img/Bordo-Genel.jpg"];
    var counter = 1;
    slideImage.src = images[0]
    console.log(slideImage.src)
    window.setInterval(changeImages,5000);

    function changeImages(){
        if(counter === images.length){
            counter = 0;
        }
        slideImage.src = images[counter];
        counter++;
        console.log(slideImage.src)
    }

}