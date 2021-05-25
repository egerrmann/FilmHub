//array of ratings
const ratings=document.querySelectorAll('.rating')
if(ratings.length>0){
    initRarings();
}

function initRarings() {
    let ratingActive, ratingValue;
    for (let index = 0; index < ratings.length; index++) {
        const rating = ratings[index];
        initRating(rating);

    }

    function initRating(rating) {
        initRatingVars(rating);
        setRatingActiveWidth();

        if (rating.classList.contains('rating_set')) {
            setRating(rating);
        }
    }

    function initRatingVars(rating) {
        ratingActive = rating.querySelector('.rating_active');
        ratingValue = rating.querySelector('.rating_value');

    }

    //Change width of active stars
    //(index=default value)
    function setRatingActiveWidth(index = ratingValue.innerHTML) {
        const ratingActiveWidth = index / 0.05;
        ratingActive.style.width = `${ratingActiveWidth}%`;
    }

    // opportunity to define ra ting
    function setRating(rating) {
        const ratingItems = rating.querySelectorAll('.rating_item');
        for (let index = 0; index < ratingItems.length; index++) {
            const ratingItem = ratingItems[index];

            ratingItem.addEventListener("mouseenter", function (e) {
                initRatingVars(rating);
                setRatingActiveWidth(ratingItem.value);
            });

            ratingItem.addEventListener("mouseleave", function (e) {
                setRatingActiveWidth();
            });

            ratingItem.addEventListener("click", function (e) {
                initRating(rating);
                setRatingValue(ratingItem.value, rating);
            })
        }
    }

    async function setRatingValue(value, rating) {
        if (!rating.classList.contains('rating_sending')) {
            rating.classList.add('rating_sending');

            //send value on server

            let response = await fetch('/Film/GetRating', {
                method: 'POST',
                body: JSON.stringify({

                    ratingType:rating.name.toString(),
                    ratingValue:value
                }),
                headers: {
                    'content-type': 'application/json'
                }
            });

            if (response.ok) {
                const result = await response.json();
                /*const newRating = result.newRating;*/

                
                setRatingActiveWidth();
                rating.classList.remove('rating_sending');
            } else {

                alert("Error");
                rating.classList.remove('rating_sending');}
        }

    }
}