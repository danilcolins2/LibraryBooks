// Проверка валидации формы при отправке
(function () {
    'use strict'

    // Получаю все формы с классом "needs-validation"
    const forms = document.querySelectorAll('.needs-validation')

    // Проходим по каждой форме
    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            if (!form.checkValidity()) {
                event.preventDefault()
                event.stopPropagation()
            }

            form.classList.add('was-validated')
        }, false)
    })
})()


const formFile = document.getElementById("formFile");
formFile.addEventListener("change", function () {
    const file = this.files[0];
    const fileType = file.type;

    if (!fileType.startsWith("image/")) {
        this.value = '';
        // Отметил input как невалидный
        this.classList.add('is-invalid');
        // Включаю подсказку
        this.nextElementSibling.style.display = "block";
    } else {
        this.classList.remove('is-invalid');
        this.nextElementSibling.style.display = "none";
    }
});


//Окошко для
$('form.needs-modal').on('submit', function (e) {
    // Создаем модальное окно
    const modal = document.createElement('div');
    modal.classList.add('modal', 'fade');
    modal.setAttribute('tabindex', '-1');
    modal.setAttribute('aria-labelledby', 'exampleModalLabel');
    modal.setAttribute('aria-hidden', 'true');
    modal.innerHTML = `
        <div class="modal-dialog">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title" id="exampleModalLabel">Подтверждение</h5>
              <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
              <p>Вы уверены, что хотите сохранить изменения?</p>
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Отмена</button>
              <button type="button" class="btn btn-primary" data-bs-dismiss="modal" id="save-button">Сохранить</button>
            </div>
          </div>
        </div>
      `;
    document.body.appendChild(modal);

    // Предотвращаем отправку формы
    e.preventDefault();

    // Покажем модальное окно
    new bootstrap.Modal(modal).show();

    const saveButton = modal.querySelector('#save-button');
    saveButton.addEventListener('click', () => {
        this.submit();
    });
});
