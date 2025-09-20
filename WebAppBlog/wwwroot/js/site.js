// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', function () {
    const editor = document.getElementById('markdown-editor');
    const preview = document.getElementById('markdown-preview');

    // 마크다운을 HTML로 변환하고 미리보기 패널에 렌더링
    function updatePreview() {
        const markdownText = editor.value;
        const htmlText = marked.parse(markdownText);
        preview.innerHTML = htmlText;
    }

    // <textarea>에 'input' 이벤트가 발생할 때마다
    // updatePreview 함수를 실행
    editor.addEventListener('input', updatePreview);

    // 페이지가 처음 로드될 때도 한 번 실행,
    // 기존에 있던 내용을 변환해 줌
    updatePreview();
});
