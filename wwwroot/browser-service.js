window.getDimensions = function() {
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
};

window.startResizeHandler = (dotNetHelper) => {
    window.addEventListener('resize', function(e) {
        dotNetHelper.invokeMethodAsync('BrowserResized', getDimensions());
    });
    return getDimensions();
};