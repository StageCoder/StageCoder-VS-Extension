
// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet", IsNullable = false)]
public partial class CodeSnippets
{

    private CodeSnippetsCodeSnippet codeSnippetField;

    /// <remarks/>
    public CodeSnippetsCodeSnippet CodeSnippet
    {
        get
        {
            return this.codeSnippetField;
        }
        set
        {
            this.codeSnippetField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet")]
public partial class CodeSnippetsCodeSnippet
{

    private CodeSnippetsCodeSnippetHeader headerField;

    private CodeSnippetsCodeSnippetSnippet snippetField;

    private string formatField;

    /// <remarks/>
    public CodeSnippetsCodeSnippetHeader Header
    {
        get
        {
            return this.headerField;
        }
        set
        {
            this.headerField = value;
        }
    }

    /// <remarks/>
    public CodeSnippetsCodeSnippetSnippet Snippet
    {
        get
        {
            return this.snippetField;
        }
        set
        {
            this.snippetField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Format
    {
        get
        {
            return this.formatField;
        }
        set
        {
            this.formatField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet")]
public partial class CodeSnippetsCodeSnippetHeader
{

    private string titleField;

    private string shortcutField;

    private string descriptionField;

    private string authorField;

    private CodeSnippetsCodeSnippetHeaderSnippetTypes snippetTypesField;

    /// <remarks/>
    public string Title
    {
        get
        {
            return this.titleField;
        }
        set
        {
            this.titleField = value;
        }
    }

    /// <remarks/>
    public string Shortcut
    {
        get
        {
            return this.shortcutField;
        }
        set
        {
            this.shortcutField = value;
        }
    }

    /// <remarks/>
    public string Description
    {
        get
        {
            return this.descriptionField;
        }
        set
        {
            this.descriptionField = value;
        }
    }

    /// <remarks/>
    public string Author
    {
        get
        {
            return this.authorField;
        }
        set
        {
            this.authorField = value;
        }
    }

    /// <remarks/>
    public CodeSnippetsCodeSnippetHeaderSnippetTypes SnippetTypes
    {
        get
        {
            return this.snippetTypesField;
        }
        set
        {
            this.snippetTypesField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet")]
public partial class CodeSnippetsCodeSnippetHeaderSnippetTypes
{

    private string snippetTypeField;

    /// <remarks/>
    public string SnippetType
    {
        get
        {
            return this.snippetTypeField;
        }
        set
        {
            this.snippetTypeField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet")]
public partial class CodeSnippetsCodeSnippetSnippet
{

    private CodeSnippetsCodeSnippetSnippetLiteral[] declarationsField;

    private CodeSnippetsCodeSnippetSnippetCode codeField;

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Literal", IsNullable = false)]
    public CodeSnippetsCodeSnippetSnippetLiteral[] Declarations
    {
        get
        {
            return this.declarationsField;
        }
        set
        {
            this.declarationsField = value;
        }
    }

    /// <remarks/>
    public CodeSnippetsCodeSnippetSnippetCode Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet")]
public partial class CodeSnippetsCodeSnippetSnippetLiteral
{

    private string idField;

    private string functionField;

    private string toolTipField;

    private string defaultField;

    private bool editableField;

    private bool editableFieldSpecified;

    /// <remarks/>
    public string ID
    {
        get
        {
            return this.idField;
        }
        set
        {
            this.idField = value;
        }
    }

    /// <remarks/>
    public string Function
    {
        get
        {
            return this.functionField;
        }
        set
        {
            this.functionField = value;
        }
    }

    /// <remarks/>
    public string ToolTip
    {
        get
        {
            return this.toolTipField;
        }
        set
        {
            this.toolTipField = value;
        }
    }

    /// <remarks/>
    public string Default
    {
        get
        {
            return this.defaultField;
        }
        set
        {
            this.defaultField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public bool Editable
    {
        get
        {
            return this.editableField;
        }
        set
        {
            this.editableField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool EditableSpecified
    {
        get
        {
            return this.editableFieldSpecified;
        }
        set
        {
            this.editableFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet")]
public partial class CodeSnippetsCodeSnippetSnippetCode
{

    private string languageField;

    private string valueField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Language
    {
        get
        {
            return this.languageField;
        }
        set
        {
            this.languageField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value
    {
        get
        {
            return this.valueField;
        }
        set
        {
            this.valueField = value;
        }
    }
}

