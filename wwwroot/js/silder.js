document.addEventListener('DOMContentLoaded', function () {
    // Start the carousel
    var myCarousel = new bootstrap.Carousel(document.getElementById('demo'), {
        interval: 3000, // Set the interval to 3 seconds (3000 milliseconds)
        pause: 'hover' // Pause on mouse hover (optional)
    });
});
