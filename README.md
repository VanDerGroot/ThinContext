# ThinContext

**ThinContext** is a small, casual experiment in compressing large language model (LLM) chat context.  
It separates full conversational output from a smaller *context summary* that gets retained for future use.  
This helps reduce context window bloat without sacrificing continuity.

> "Remember just enough. Keep the signal, lose the noise."

---

## 🧠 What It Does

When you send a prompt to the model:

- It responds with **two things**:
  - `"message"` – the full reply, for display.
  - `"context"` – a short factual summary, for memory.
- Your **chat history stores both**, but only the `"context"` version of AI replies is fed back to the model.
- **User messages** are always included in full.

This creates a lightweight but coherent conversation thread that can go on much longer within a limited token budget.

---

## 💡 Why It Exists

I built this out of curiosity and necessity. Context limits are frustrating when you're trying to have thoughtful, ongoing conversations with an LLM. This approach felt simple enough to try—and it worked better than expected.

It's not perfect, and it's not polished. But it's something.

---

## 🔧 Requirements

- A locally running LLM (tested with `mistral-7b`)
- .NET 6 or later
- JSON schema support in your model's completion endpoint

---

## 🚧 Limitations

- Not fully tested
- No weighting/prioritization (yet)
- No built-in token accounting
- Very basic fallback error handling

That said—it works! The core idea is small, but potentially useful.

---

## 💬 Feedback Welcome

I'm genuinely curious what others think about this.  
If you have ideas, suggestions, or just want to discuss the concept, feel free to:

- Open an [issue](https://github.com/VanDerGroot/ThinContext/issues)
- Start a [discussion](https://github.com/VanDerGroot/ThinContext/discussions) *(if enabled)*

---

## 📜 License

Released under [CC BY 4.0](https://creativecommons.org/licenses/by/4.0/).  
Use it, remix it, build on it—just give credit.

---

## 🧭 Future Ideas

- Emotional weighting and salience control
- Token tracking and optimization reporting
- Visualizer for “visible vs. context” summaries
- Plugin wrapper for other models

---

Thanks for checking this out.  
Maybe it's just a little experiment.  
Maybe it’s the start of something more.
