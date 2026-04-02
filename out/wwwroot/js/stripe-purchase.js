// Stripe Purchase Handler with Auto-Click
(function() {
    'use strict';

    // Configuration
    const PURCHASE_CONFIG = {
        url: 'https://app.zerobytebd.com/business-erp',
        productId: 'business-erp',
        productPrice: 69,
        autoClickSelector: '.btn-buy-now, .btn-purchase',
        timeout: 2000
    };

    // Main purchase handler
    window.handleStripePurchase = function() {
        const btn = document.getElementById('btnStripePurchase');
        
        // Show loading state
        setButtonLoading(btn, true);
        
        // Open purchase page
        const purchaseWindow = openPurchaseWindow();
        
        // Monitor window and attempt auto-click
        if (purchaseWindow) {
            monitorPurchaseWindow(purchaseWindow, btn);
        } else {
            // Fallback: direct navigation
            window.location.href = PURCHASE_CONFIG.url;
        }
    };

    // Set button loading state
    function setButtonLoading(btn, isLoading) {
        if (isLoading) {
            btn.dataset.originalHtml = btn.innerHTML;
            btn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> <span>Processing...</span>';
            btn.disabled = true;
        } else {
            btn.innerHTML = btn.dataset.originalHtml;
            btn.disabled = false;
        }
    }

    // Open purchase window
    function openPurchaseWindow() {
        const width = 900;
        const height = 700;
        const left = (screen.width - width) / 2;
        const top = (screen.height - height) / 2;
        
        const features = `width=${width},height=${height},left=${left},top=${top},` +
                        'resizable=yes,scrollbars=yes,status=yes';
        
        return window.open(PURCHASE_CONFIG.url, 'StripePurchase', features);
    }

    // Monitor purchase window
    function monitorPurchaseWindow(purchaseWindow, btn) {
        let attempts = 0;
        const maxAttempts = 10;
        
        const checkInterval = setInterval(function() {
            attempts++;
            
            // Check if window is closed
            if (purchaseWindow.closed) {
                clearInterval(checkInterval);
                setButtonLoading(btn, false);
                showNotification('Purchase window closed', 'info');
                return;
            }
            
            // Try to trigger auto-click
            try {
                if (purchaseWindow.document && purchaseWindow.document.readyState === 'complete') {
                    const purchaseBtn = purchaseWindow.document.querySelector(PURCHASE_CONFIG.autoClickSelector);
                    if (purchaseBtn) {
                        purchaseBtn.click();
                        clearInterval(checkInterval);
                        showNotification('Purchase initiated successfully!', 'success');
                        setTimeout(() => setButtonLoading(btn, false), 2000);
                    }
                }
            } catch (e) {
                // Cross-origin restriction - expected
                console.log('Cross-origin restriction - user will manually click purchase');
            }
            
            // Stop after max attempts
            if (attempts >= maxAttempts) {
                clearInterval(checkInterval);
                setButtonLoading(btn, false);
                showNotification('Purchase window opened. Please complete your purchase.', 'info');
            }
        }, 500);
    }

    // Show notification
    function showNotification(message, type) {
        if (typeof toastr !== 'undefined') {
            toastr[type](message);
        } else if (typeof Swal !== 'undefined') {
            Swal.fire({
                icon: type,
                title: message,
                toast: true,
                position: 'top-end',
                showConfirmButton: false,
                timer: 3000
            });
        } else {
            alert(message);
        }
    }

    // Listen for purchase completion messages
    window.addEventListener('message', function(event) {
        // Verify origin for security
        if (event.origin === 'https://app.zerobytebd.com') {
            if (event.data.status === 'purchase_completed') {
                showNotification('Purchase completed successfully!', 'success');
                // Reload or update UI
                setTimeout(() => location.reload(), 2000);
            } else if (event.data.status === 'purchase_cancelled') {
                showNotification('Purchase was cancelled', 'warning');
            }
        }
    });

})();
