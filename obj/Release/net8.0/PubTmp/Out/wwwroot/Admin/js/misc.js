(function ($) {
    'use strict';

    $(function () {
        var body = $('body');
        var sidebar = $('.sidebar');

        // Get the current path (without query parameters)
        var currentPath = location.pathname; // e.g., "/admin/Participant/list"
        var controller = currentPath.split("/")[2]; // e.g., "Participant"

        // Clear any existing active classes
        $('.nav-item').removeClass('active');

        // Debugging output
        console.log("Current Controller:", controller);

        // Add active class to the correct nav-link based on the controller
        $('.nav li a', sidebar).each(function () {
            var $this = $(this);
            var href = $this.attr('href') || ""; // Make sure href is defined
            var hrefController = href.split("/")[2]; // Extract the controller from the href

            // Debugging output
            console.log("Checking href controller:", hrefController);

            // Check if the href controller matches the current controller
            if (hrefController === controller) {
                // Add active class to the matching nav-item
                $this.closest('.nav-item').addClass('active');
                console.log("Active class added to:", href);
            }
        });

        // Sidebar collapse functionality
        sidebar.on('show.bs.collapse', '.collapse', function () {
            sidebar.find('.collapse.show').collapse('hide');
        });

        // Minimize sidebar on button click
        $('[data-toggle="minimize"]').on("click", function () {
            if (body.hasClass('sidebar-toggle-display') || body.hasClass('sidebar-absolute')) {
                body.toggleClass('sidebar-hidden');
            } else {
                body.toggleClass('sidebar-icon-only');
            }
        });

        // Optionally apply styles for scrollbars and other features
        applyStyles();

        function applyStyles() {
            // Apply perfect scrollbar if necessary
            if (!body.hasClass("rtl")) {
                if ($('.settings-panel .tab-content .tab-pane.scroll-wrapper').length) {
                    new PerfectScrollbar('.settings-panel .tab-content .tab-pane.scroll-wrapper');
                }
                if ($('.chats').length) {
                    new PerfectScrollbar('.chats');
                }
                if (body.hasClass("sidebar-fixed")) {
                    new PerfectScrollbar('#sidebar .nav');
                }
            }
        }
    });
})(jQuery);