$(() => {

    const questionId = $("#ids").data('question-id');
    const userId = $("#ids").data('user-id');

    $("#submit-answer").on('click', function () {
        const answer = {                 //get the answer with question id, user id, and text
            Text: $("#answer-text").val(),
            QuestionId: questionId,
            UserId: userId
        }
        $.post('/home/postAnswer', answer, function () {
            $("#answer-text").val('');
            $("#no-answers").hide();     //in the event it's the first answer, it'll hide the span that says no answers
            const userEmail = $("#ids").data('user-email');
            AddNewAnswer(userEmail, answer.Text);
        });
    });

    function AddNewAnswer(userEmail, text) {            //adds the new answer just submitted to the page
        $("#answers").append(`<div class="row">
                    <div class="col-md-6 offset-3 card card-body bg-light">
                        <div class="container">
                            <p>${text}</p>
                            <p>Submitted by: ${userEmail}</p>
                        </div>
                    </div>
                </div>
                <br />`)
    }

    $("#btn-like").on('click', function () {
        //should add a like to that question and disable the button, need QuestionId and UserId
        const LikesQuestions = {
            UserId: userId,
            QuestionId: questionId
        }
        $.post('/home/likeQuestion', LikesQuestions, function () {
            $.get(`/home/GetUpdatedLikes?questionId=${questionId}`, function (questionsLikes) {
                $("#current-likes").text(`Likes: ${questionsLikes}`);
            });
            //update the like on the page
        });
        $("#btn-like").prop('disabled', true);
    });

   
});