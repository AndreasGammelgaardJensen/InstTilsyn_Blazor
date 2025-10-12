function openInNewTab(url) {
    window.open(url, '_blank');
}

// Register shrink behavior for hero header
window.registerHeroShrink = function (id) {
    const el = document.getElementById(id);
    if (!el) return;
    const threshold = 140; // px scroll before shrink
    function onScroll() {
        if (window.scrollY > threshold) {
            el.classList.add('shrunk');
        } else {
            el.classList.remove('shrunk');
        }
    }
    window.addEventListener('scroll', onScroll, { passive: true });
    // initial state
    onScroll();
    // store cleanup
    el.__shrinkCleanup = () => window.removeEventListener('scroll', onScroll);
};

window.unregisterHeroShrink = function(id){
    const el = document.getElementById(id);
    if(el && el.__shrinkCleanup){
        el.__shrinkCleanup();
        delete el.__shrinkCleanup;
    }
};