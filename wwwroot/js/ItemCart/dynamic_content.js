const GetItemDivTemplate = (data) => {
    return `
        <div id="divItemContent${data.Id}" class="info-box col-12 col-sm-12" onmouseover="showButton(this)" onmouseout="hideButton(this)">
            <span class="info-box-icon bg-info elevation-1">
                <img src="${data.ImageURL}" class="img-circle elevation-1" alt="Asset Image">
            </span>
            <div class="info-box-content">
                <span class="info-box-text">
                    <a href="#" onclick=ViewItem(${data.Id}); class="view-item-link">${data.Name}</a>
                </span>
                <span class="info-box-number">
                    Price:&nbsp;${data.SellPrice} &nbsp;(${data.Quantity})
                </span>
                <span class="info-box-text" hidden>
                    <label class="control-label small">${data.Barcode}</label>
                    <label class="control-label small">${data.CategoriesName}</label>
                </span>
                
                <button class="add-to-cart-button btn btn-primary shadow-sm" onclick="AddtoCart('${data.Id}')" style="display: none; color: white; font-weight: bold; border-radius: 20px; padding: 8px 16px;">
                    <i class="fas fa-cart-plus"></i> Add to Cart
                </button>
            </div>
        </div>
    `;
};

// Show and hide button functions
const showButton = (element) => {
    const button = element.querySelector('.add-to-cart-button');
    if (button) button.style.display = 'inline-block';
};

const hideButton = (element) => {
    const button = element.querySelector('.add-to-cart-button');
    if (button) button.style.display = 'none';
};
