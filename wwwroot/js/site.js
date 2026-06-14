document.addEventListener('DOMContentLoaded', () => {
    const currentPath = window.location.pathname.toLowerCase();

    document.querySelectorAll('.nav-link').forEach((link) => {
        const href = link.getAttribute('href')?.toLowerCase() || '';
        if (href && currentPath.includes(href.replace('/', '')) && href !== '/') {
            link.classList.add('active');
        } else if (href === '/' && (currentPath === '/' || currentPath.endsWith('/home') || currentPath.endsWith('/home/index'))) {
            link.classList.add('active');
        }
    });

    const header = document.getElementById('siteHeader');
    if (header) {
        const onScroll = () => {
            if (window.scrollY > 40) {
                header.classList.add('scrolled');
            } else {
                header.classList.remove('scrolled');
            }
        };
        window.addEventListener('scroll', onScroll, { passive: true });
        onScroll();
    }
});
